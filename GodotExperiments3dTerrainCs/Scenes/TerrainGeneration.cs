using Godot;

using System;
using System.Linq;

namespace GodotExperiments.Terrain3DCSharp;

[Tool]
public partial class TerrainGeneration : Node
{
  [Export]
  public MeshInstance3D InitialMeshInstance { get; set; }


  [Export]
  public Node[] TerrainClasses { get; set; }

  public override void _Ready()
  {
    base._Ready();
    InitialMeshInstance.Visible = false;

    GenerateMesh();
  }

  public void GenerateMesh()
  {
    foreach (var item in TerrainClasses)
    {
      GD.Print(item);

      if (item is TerrainClassBase t)
      {
        AddChild(GenerateMesh(t));
      }
    }
  }

  public MeshInstance3D GenerateMesh(TerrainClassBase terrainClass)
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
      vertex.Y = GetNoiseY(terrainClass, vertex.X, vertex.Z) ?? -1;
      meshDataTool.SetVertex(i, vertex);
    }

    arrayPlane.ClearSurfaces();
    meshDataTool.CommitToSurface(arrayPlane);

    surfaceTool.Begin(Mesh.PrimitiveType.Points);
    surfaceTool.CreateFrom(arrayPlane, 0);
    surfaceTool.GenerateNormals();

    var newMeshInstance = new MeshInstance3D
    {
      Mesh = surfaceTool.Commit(),
      Visible = true
    };
    newMeshInstance.CreateTrimeshCollision();

    return newMeshInstance;
  }

  private float? GetNoiseY(TerrainClassBase terrainClass, float x, float y) => terrainClass.FastNoiseLite?.GetNoise2D(x, y) * 50;
}
