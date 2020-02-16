using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DelugeMidi
{
	class FileProcessor
	{
		private int currentChannel = 0;
		private int currentColumn = 0;

		public XDocument Process(string path, DelugeMidi delugeMidi)
		{
			Console.WriteLine("{0}",
					path);
			var doc = XDocument.Load(path);
			var instruments = doc.Descendants("instruments").Single();

			var synthLayouts = new Dictionary<string, Layout>()
			{
				{"fm", delugeMidi.FMLayout},
				{"ringmod", delugeMidi.RingModLayout},
				{"subtractive", delugeMidi.SubtractiveLayout},
			};

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "sound")
				.ForEach(synth =>
			{
				var mode = synth.Attribute("mode").Value;
				var layout = synthLayouts[mode];
				var knobs = RenderKnobs(
					layout.Page(),
					delugeMidi.ControllerLayout.Page(),
					currentChannel);
				addKnobs(synth, knobs);
				Console.WriteLine(
					"  Synth: name={1};layout={2};channel={3}",
					path,
					GetInstrumentName(synth),
					mode,
					currentChannel);
				currentChannel += 1;
			});

			instruments.Elements()
				.OrderBy(GetInstrumentName)
				.Where(instrument => instrument.Name.LocalName == "kit")
				.ForEach(kit =>
				{
					var sounds = kit.Element("soundSources").Elements("sound").ToArray();
					sounds.ForEach(sound =>
					{
						if (currentChannel > 15)
						{
							return;
						}
						var knobs = RenderKnobs(
							delugeMidi.KitColLayout.Column(0),
							delugeMidi.ControllerLayout.Column(currentColumn),
							currentChannel);
						addKnobs(sound, knobs);
						Console.WriteLine(
							"  Kit: name={1};sound={2};channel={3};column={4}",
							path,
							GetInstrumentName(kit),
							GetInstrumentName(sound),
							currentChannel,
							currentColumn);
						currentColumn += 1;
						if (currentColumn == 8)
						{
							currentChannel += 1;
							currentColumn = 0;
						}
					});
					if (currentColumn > 0)
					{
						currentChannel += 1;
						currentColumn = 0;
					}
				});
			return doc;
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

		private void addKnobs(XElement instrument, XElement knobs)
		{
			var current = instrument.Elements("midiKnobs");
			if (current != null)
			{
				current.Remove();
			}
			instrument.Add(knobs);
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