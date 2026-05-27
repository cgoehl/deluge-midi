const params = [
	"---",
	"arpGate",
	"arpRate",
	"bass",
	"bassFreq",
	"bassProbability",
	"bitcrushAmount",
	"carrier1Feedback",
	"carrier2Feedback",
	"chordPolyphony",
	"chordProbability",
	"compressorShape",
	"compressorThreshold",
	"delayFeedback",
	"delayRate",
	"env1Attack",
	"env1Decay",
	"env1Release",
	"env1Sustain",
	"env2Attack",
	"env2Decay",
	"env2Release",
	"env2Sustain",
	"env3Attack",
	"env3Decay",
	"env3Release",
	"env3Sustain",
	"env4Attack",
	"env4Decay",
	"env4Release",
	"env4Sustain",
	"hpfFrequency",
	"hpfMorph",
	"hpfResonance",
	"lfo1Rate",
	"lfo2Rate",
	"lfo3Rate",
	"lfo4Rate",
	"lpfFrequency",
	"lpfMorph",
	"lpfResonance",
	"modFXDepth",
	"modFXFeedback",
	"modFXOffset",
	"modFXRate",
	"modulator1Feedback",
	"modulator1Pitch",
	"modulator1Volume",
	"modulator2Feedback",
	"modulator2Pitch",
	"modulator2Volume",
	"noiseVolume",
	"noteProbability",
	"oscAPhaseWidth",
	"oscAPitch",
	"oscAVolume",
	"oscAWavetablePosition",
	"oscAWavetablePosition",
	"oscBPhaseWidth",
	"oscBPitch",
	"oscBVolume",
	"pan",
	"pitch",
	"pitchAdjust",
	"portamento",
	"ratchetAmount",
	"ratchetProbability",
	"reverbAmount",
	"reverseProbability",
	"rhythm",
	"sampleRateReduction",
	"sequenceLength",
	"sidechainCompressorVolume",
	"spreadGate",
	"spreadOctave",
	"spreadVelocity",
	"stutterRate",
	"treble",
	"trebleFreq",
	"volume",
	"volumePostFX",
	"waveFoldH"];

document.addEventListener("readystatechange", onReady);

const lcxl = {
	width: 8,
	height: 4,
	ccNumbers: [
		13, 14, 15, 16, 17, 18, 19, 20,
		29, 30, 31, 32, 33, 34, 35, 36,
		49, 50, 51, 52, 53, 54, 55, 56,
		77, 78, 79, 80, 81, 82, 83, 84,
	],
}

function getSavedValue(cc)
{
	return localStorage.getItem(`saved_${cc}`) || '---';
}

function saveValue(cc, value)
{
	if (value != '---')
	{
		localStorage.setItem(`saved_${cc}`, value);
	}
	else
	{
		localStorage.removeItem(`saved_${cc}`);
	}
}

function generateXml(controller)
{
	const container = document.getElementById("generated_xml");
	container.innerText = params.length;
}

function createInput(controller, x, y)
{
	const cc = controller.ccNumbers[x + y * controller.width];
	const input = document.createElement("select");
	const value = getSavedValue(cc);
	console.log(cc, value);
	params.forEach(p => {
  	const option = document.createElement('option');
		option.value = p;
		option.text = p;
		input.append(option);
	});

	input.id = cc;
	input.value = value;
	input.addEventListener('change', function(e) {
		const value = event.target.value;
		saveValue(cc, value);
		generateXml(controller);
	});
	return input;
}

function main(controller)
{
	const root = document.getElementById('form_grid');
	for (let y = 0; y < controller.height; y++) {
		const row = document.createElement("div");
		row.classList.add('row')
		for (let x = 0; x < controller.width; x++) {
			row.append(createInput(controller, x, y));
		}
		root.append(row);
	}
}

function onReady() {
	main(lcxl);
}