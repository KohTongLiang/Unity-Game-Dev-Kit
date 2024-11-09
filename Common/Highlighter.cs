using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace GameCore
{
    public enum HighlighterType
    {
        Outline
    }

    public class Highlighter : Singleton<Highlighter>
    {
        [SerializeField] private Material outlineMaterial;

        private readonly Dictionary<string, MeshRenderer> meshes = new();

        public void ApplyOutlineShader(string id, MeshRenderer mesh)
        {
            if (meshes.ContainsKey(id)) return;
            var materials = mesh.materials;
            var newMaterials = new Material[materials.Length + 1];
            materials.CopyTo(newMaterials, 0);
            newMaterials[^1] = outlineMaterial;
            mesh.materials = newMaterials;
            meshes.Add(id, mesh);
        }

        public void RemoveOutlineShader(string id)
        {
            if (!meshes.TryGetValue(id, out var mesh)) return;

            var materials = mesh.materials;
            var newMaterials = new Material[materials.Length - 1];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = materials[i];
            }

            mesh.materials = newMaterials;
            meshes.Remove(id);
        }
    }
}