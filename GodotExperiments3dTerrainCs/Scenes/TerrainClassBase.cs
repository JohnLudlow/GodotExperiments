using Godot;

namespace GodotExperiments.Terrain3DCSharp;

public partial class TerrainClassBase : Node
{
  [Export]
  public FastNoiseLite FastNoiseLite { get; set; }

  [Export]
  public float MinElevation { get; set; }
  
  [Export]
  public float MaxElevation { get; set; }
}
