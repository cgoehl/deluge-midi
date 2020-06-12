using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DelugeMidi
{
	class FileProcessor
	{
		private readonly Config _config;
		private readonly DelugeMidi _delugeMidi;
		private readonly Dictionary<string, Layout> _synthLayouts;
		private int _currentChannel;
		private int _currentColumn;

		public FileProcessor(Config config, DelugeMidi delugeMidi)
		{
			_config = config;
			_delugeMidi = delugeMidi;
			_synthLayouts = new Dictionary<string, Layout>()
			{
				{"fm", _delugeMidi.FMLayout},
				{"ringmod", _delugeMidi.RingModLayout},
				{"subtractive", _delugeMidi.SubtractiveLayout},
			};
			_currentChannel = _config.FirstChannel;
		}

		public void Process(string path)
		{
			Console.WriteLine("{0}",
					path);
			var doc = XDocument.Load(path);
			var instruments = doc.Descendants("instruments").Single();

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "sound")
				.TakeWhile(_ => HasChannelsLeft)
				.ForEach(AddSynth);

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "kit")
				.TakeWhile(_ => HasChannelsLeft)
				.ForEach(AddKit);

			doc.Save(path);
		}

		void AddSynth(XElement synth)
		{
			var mode = synth.Attribute("mode").Value;
			var layout = _synthLayouts[mode];
			var knobs = RenderKnobs(
				layout.Page(), 
				_delugeMidi.ControllerKnobsLayout.Page(), 
				_currentChannel);
			synth.ReplaceChildElement(knobs);

			synth.SetAttributeValue("inputMidiChannel", _currentChannel);

			Console.WriteLine(
				"  Synth: name={0};layout={1};channel={2}", 
				GetInstrumentName(synth), 
				mode, 
				_currentChannel);

			_currentChannel += 1;
		}

		void AddKit(XElement kit)
		{
			kit
				.Element("soundSources")
				.Elements("sound")
				.TakeWhile(_ => HasChannelsLeft)
				.ForEach(sound => AddKitRow(kit, sound));
			if (_currentColumn > 0)
			{
				_currentChannel += 1;
				_currentColumn = 0;
			}
		}

		void AddKitRow(XElement kit, XElement sound)
		{
			var knobs = RenderKnobs(
				_delugeMidi.KitColLayout.Column(0), 
				_delugeMidi.ControllerKnobsLayout.Column(_currentColumn), 
				_currentChannel);
			sound.ReplaceChildElement(knobs);

			AddKitRowNote(sound);

			Console.WriteLine(
				"  Kit: name={0};sound={1};channel={2};column={3}",
				GetInstrumentName(kit), 
				GetInstrumentName(sound), 
				_currentChannel, 
				_currentColumn);

			_currentColumn += 1;
			if (!HasColumnsLeft)
			{
				_currentChannel += 1;
				_currentColumn = 0;
			}
		}

		private void AddKitRowNote(XElement sound)
		{
			var notes = _delugeMidi.ControllerNotesLayout;
			if (notes.Width > _currentColumn)
			{
				var elem = new XElement("midiInput");
				elem.SetAttributeValue("channel", _currentChannel);
				elem.SetAttributeValue("note", notes.At(_currentColumn, 0));
				sound.ReplaceChildElement(elem);
			}
		}

		private bool HasChannelsLeft => _currentChannel < 16;
		private bool HasColumnsLeft => _currentColumn < _delugeMidi.ControllerKnobsLayout.Width;

		public XElement RenderKnobs(IEnumerable<string> contents, IEnumerable<string> controller, int channel)
		{
			var midiKnobs = new XElement("midiKnobs");
			Enumerable.Zip(
				contents,
				controller,
				(param, cc) =>
				{
					var midiKnob = new XElement("midiKnob");
					midiKnob.SetAttributeValue("channel", channel);
					midiKnob.SetAttributeValue("ccNumber", cc);
					midiKnob.SetAttributeValue("relative", 0);
					midiKnob.SetAttributeValue("controlsParam", param);
					return midiKnob;
				})
				.ForEach(midiKnobs.Add);
			return midiKnobs;
		}

		private String GetInstrumentName(XElement instrument)
		{
			return new[]
			{
				instrument.Attribute("presetSlot"),
				instrument.Attribute("presetName")
			}
			.Where(attr => attr != null)
			.Select(attribute => attribute.Value)
			.FirstOrDefault();
		}
	}
}