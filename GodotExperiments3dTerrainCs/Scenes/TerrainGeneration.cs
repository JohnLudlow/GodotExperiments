using Godot;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodotExperiments.Terrain3DCSharp;

[Tool]
public partial class TerrainGeneration : Node
{
  readonly Random _rng = new();

  [Export]
  public MeshInstance3D InitialMeshInstance { get; set; }

  public MeshInstance3D ActiveMeshInstance { get; set; } = new MeshInstance3D();

  [Export]
  public TerrainClassBase TerrainConfig { get; set; }

  [Export]
  public TerrainClassBase[] TerrainClasses { get; set; }

  public override void _Ready()
  {
    base._Ready();
    InitialMeshInstance.Visible = false;

    AddChild(ActiveMeshInstance);

    if (Engine.IsEditorHint())
    {
      GenerateMesh();
    }
  }

  public void GenerateMesh()
  {
    foreach (var item in TerrainClasses)
    {
      GenerateMesh(item);
    }
  }

  public void GenerateMesh(TerrainClassBase terrainClassParameters)
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
      vertex.Y = GetNoiseY(terrainClassParameters, vertex.X, vertex.Z) ?? -1;
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

  private static float? GetNoiseY(TerrainClassBase terrainClassParameters, float x, float y) 
    => terrainClassParameters?.FastNoiseLite?.GetNoise2D(x, y) * 50;
}
