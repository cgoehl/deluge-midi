using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DelugeMidi.FW;

namespace DelugeMidi
{
	class FileProcessor
	{
		private readonly Config _config;
		private readonly DelugeMidi _delugeMidi;
		private readonly Dictionary<string, Layout> _synthLayouts;
		private int _currentChannel;
		private int _currentColumn = 0;
		

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

		public XDocument Process(string path)
		{
			Console.WriteLine("{0}",
					path);
			var doc = XDocument.Load(path);
			var instruments = doc.Descendants("instruments").Single();

			

			

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "sound")
				.ForEach(AddSynth);

			

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "kit")
				.ForEach(AddKit);
			return doc;
		}

		void AddSynth(XElement synth)
			{
				var mode = synth.Attribute("mode").Value;
				var layout = _synthLayouts[mode];
				var knobs = RenderKnobs(layout.Page(), _delugeMidi.ControllerKnobsLayout.Page(), _currentChannel);
				synth.ReplaceChildElement(knobs);
				synth.SetAttributeValue("inputMidiChannel", _currentChannel);
				Console.WriteLine("  Synth: name={0};layout={1};channel={2}", GetInstrumentName(synth), mode, _currentChannel);
				_currentChannel += 1;
			}

			void AddKit(XElement kit)
			{
				var sounds = kit.Element("soundSources").Elements("sound").ToArray();
				sounds.ForEach(sound => AddKitRow(kit, sound));
				if (_currentColumn > 0)
				{
					_currentChannel += 1;
					_currentColumn = 0;
				}
			}

			void AddKitRow(XElement kit, XElement sound)
			{
				if (_currentChannel > 15)
				{
					return;
				}

				var knobs = RenderKnobs(_delugeMidi.KitColLayout.Column(0), _delugeMidi.ControllerKnobsLayout.Column(_currentColumn), _currentChannel);
				sound.ReplaceChildElement(knobs);

				//<midiInput channel="8" note="42" />

				Console.WriteLine("  Kit: name={0};sound={1};channel={2};column={3}", GetInstrumentName(kit), GetInstrumentName(sound), _currentChannel, _currentColumn);
				_currentColumn += 1;
				if (_currentColumn == 8)
				{
					_currentChannel += 1;
					_currentColumn = 0;
				}
			}

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