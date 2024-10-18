using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Godot;

namespace GodotExperiments.Terrain3DCSharp.Ui;

public static class ButtonExtensions
{
  public static OptionButton WithOptions<TEnum>(this OptionButton button) where TEnum : Enum
  {
    var enumValues = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

    for (var i = 0; i < enumValues.Count(); i++)
    {
      button.AddItem(Enum.GetName(typeof(TEnum), enumValues.ElementAt(i)), Convert.ToInt32(enumValues.ElementAt(i)));
    }

    button.ForceUpdateTransform();
    return button;
  }
}

public partial class Ui : Panel
{
  [Export]
  public TerrainGeneration TerrainGeneration { get; set; }

  OptionButton _noiseTypeControl;
  SpinBox _noiseSeedControl;
  SpinBox _noiseFreqControl;
  
  OptionButton _noiseFractalTypeControl;
  SpinBox _noiseFractalLacControl;
  SpinBox _noiseFractalGainControl;
  SpinBox _noiseFractalWeightedStressControl;

  OptionButton _noiseCellularFunctionControl;
  SpinBox _noiseCellularJitterControl;
  OptionButton _noiseCellularReturnControl;

  CheckBox _noiseDomainWarpEnabledControl;
  OptionButton _noiseDomainWarpTypeControl;
  SpinBox _noiseDomainWarpAmplitudeControl;
  SpinBox _noiseDomainWarpFrequencyControl;
  
  OptionButton _noiseDomainWarpFractalTypeControl;
  SpinBox _noiseDomainWarpFractalOctaveControl;
  SpinBox _noiseDomainWarpFractalLacControl;
  SpinBox _noiseDomainWarpFractalGainControl;
  

  public override void _Ready()
  {
    base._Ready();

    _noiseTypeControl                     ??= GetNode<OptionButton>("NoisePanel/NoiseType/Value").WithOptions<FastNoiseLite.NoiseTypeEnum>();
    _noiseSeedControl                     ??= GetNode<SpinBox>("NoisePanel/NoiseSeed/Value");
    _noiseFreqControl                     ??= GetNode<SpinBox>("NoisePanel/NoiseFreq/Value");

    _noiseFractalTypeControl              ??= GetNode<OptionButton>("NoisePanel/NoiseFractalType/Value").WithOptions<FastNoiseLite.FractalTypeEnum>();
    _noiseFractalLacControl               ??= GetNode<SpinBox>("NoisePanel/NoiseFractalLacunarity/Value");
    _noiseFractalGainControl              ??= GetNode<SpinBox>("NoisePanel/NoiseFractalGain/Value");
    _noiseFractalWeightedStressControl    ??= GetNode<SpinBox>("NoisePanel/NoiseFractalWeightedStress/Value");

    _noiseCellularFunctionControl         ??= GetNode<OptionButton>("NoisePanel/NoiseCellularFunction/Value").WithOptions<FastNoiseLite.CellularDistanceFunctionEnum>();
    _noiseCellularJitterControl           ??= GetNode<SpinBox>("NoisePanel/NoiseCellularJitter/Value");
    _noiseCellularReturnControl           ??= GetNode<OptionButton>("NoisePanel/NoiseCellularReturn/Value").WithOptions<FastNoiseLite.CellularReturnTypeEnum>();

    _noiseDomainWarpEnabledControl        ??= GetNode<CheckBox>("NoisePanel/NoiseDomainWarpEnabled/Value");
    _noiseDomainWarpTypeControl           ??= GetNode<OptionButton>("NoisePanel/NoiseDomainWarpType/Value").WithOptions<FastNoiseLite.DomainWarpTypeEnum>();
    _noiseDomainWarpAmplitudeControl      ??= GetNode<SpinBox>("NoisePanel/NoiseDomainWarpAmplitude/Value");
    _noiseDomainWarpFrequencyControl      ??= GetNode<SpinBox>("NoisePanel/NoiseDomainWarpFreq/Value");

    _noiseDomainWarpFractalTypeControl    ??= GetNode<OptionButton>("NoisePanel/NoiseDomainWarpFractalType/Value").WithOptions<FastNoiseLite.DomainWarpFractalTypeEnum>();
    _noiseDomainWarpFractalOctaveControl  ??= GetNode<SpinBox>("NoisePanel/NoiseDomainWarpFractalOctave/Value");
    _noiseDomainWarpFractalLacControl     ??= GetNode<SpinBox>("NoisePanel/NoiseDomainWarpFractalLacunarity/Value");
    _noiseDomainWarpFractalGainControl    ??= GetNode<SpinBox>("NoisePanel/NoiseDomainWarpFractalGain/Value");
  }



  public void OnUiUpdate()
  {
    TerrainGeneration.MeshNoise.NoiseType                    = (FastNoiseLite.NoiseTypeEnum)_noiseTypeControl.Selected;
    TerrainGeneration.MeshNoise.Seed                         = (int)_noiseSeedControl.Value;
    TerrainGeneration.MeshNoise.Frequency                    = (float)_noiseFreqControl.Value;

    TerrainGeneration.MeshNoise.FractalType                  = (FastNoiseLite.FractalTypeEnum)_noiseFractalTypeControl.Selected;
    TerrainGeneration.MeshNoise.FractalLacunarity            = (float)_noiseFractalLacControl.Value;
    TerrainGeneration.MeshNoise.FractalGain                  = (float)_noiseFractalGainControl.Value;
    TerrainGeneration.MeshNoise.FractalWeightedStrength      = (float)_noiseFractalWeightedStressControl.Value;

    TerrainGeneration.MeshNoise.CellularDistanceFunction     = (FastNoiseLite.CellularDistanceFunctionEnum)_noiseCellularFunctionControl.Selected;
    TerrainGeneration.MeshNoise.CellularJitter               = (float)_noiseCellularJitterControl.Value;
    TerrainGeneration.MeshNoise.CellularReturnType           = (FastNoiseLite.CellularReturnTypeEnum)_noiseCellularReturnControl.Selected;

    TerrainGeneration.MeshNoise.DomainWarpEnabled            = _noiseDomainWarpEnabledControl.ButtonPressed;
    TerrainGeneration.MeshNoise.DomainWarpType               = (FastNoiseLite.DomainWarpTypeEnum)_noiseDomainWarpTypeControl.Selected;
    TerrainGeneration.MeshNoise.DomainWarpAmplitude          = (float)_noiseDomainWarpAmplitudeControl.Value;
    TerrainGeneration.MeshNoise.DomainWarpFrequency          = (float)_noiseDomainWarpFrequencyControl.Value;

    TerrainGeneration.MeshNoise.DomainWarpFractalType        = (FastNoiseLite.DomainWarpFractalTypeEnum)_noiseDomainWarpFractalTypeControl.Selected;
    TerrainGeneration.MeshNoise.DomainWarpFractalOctaves     = (int)_noiseDomainWarpFractalOctaveControl.Value;
    TerrainGeneration.MeshNoise.DomainWarpFractalLacunarity  = (float)_noiseDomainWarpFractalLacControl.Value;
    TerrainGeneration.MeshNoise.DomainWarpFractalGain        = (float)_noiseDomainWarpFractalGainControl.Value;
    TerrainGeneration.GenerateMesh();
  }
}
