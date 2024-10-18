using Godot;

using System;

namespace GodotExperiments.Terrain3DCSharp;

public partial class TerrainGeneration : Node
{
  readonly Random _rng = new();

  [Export]
  public MeshInstance3D InitialMeshInstance { get; set; }

  public MeshInstance3D ActiveMeshInstance { get; set; } = new MeshInstance3D();

  [Export]
  public FastNoiseLite MeshNoise { get; set; }

  public override void _Ready()
  {
    base._Ready();
    InitialMeshInstance.Visible = false;

    AddChild(ActiveMeshInstance);
  }

  public void GenerateMesh()
  {
    var surfaceTool = new SurfaceTool();
    var meshDataTool = new MeshDataTool();

    surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
    surfaceTool.CreateFrom(InitialMeshInstance.Mesh, 0);

    var arrayPlane = surfaceTool.Commit();
    meshDataTool.CreateFromSurface(arrayPlane, 0);

    for (var i = 0; i < meshDataTool.GetVertexCount() - 1; i++)
    {
      var vertex = meshDataTool.GetVertex(i);
      vertex.Y = GetNoiseY(vertex.X, vertex.Z);
      meshDataTool.SetVertex(i, vertex);
    }

    arrayPlane.ClearSurfaces();
    meshDataTool.CommitToSurface(arrayPlane);

    surfaceTool.Begin(Mesh.PrimitiveType.Points);
    surfaceTool.CreateFrom(arrayPlane, 0);
    surfaceTool.GenerateNormals();

    ActiveMeshInstance.Mesh = surfaceTool.Commit();
    ActiveMeshInstance.CreateTrimeshCollision();
  }

  private float GetNoiseY(float x, float y)
  {
    return MeshNoise.GetNoise2D(x, y) * 25;
  }
}
