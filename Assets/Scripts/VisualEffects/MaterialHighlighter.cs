using UnityEngine;

namespace KitchenChaos.VisualEffects
{
    /// <summary>
    /// Component to Highlight/UnHighlight a material using the Lit Highlighter Shader.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class MaterialHighlighter : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Renderer renderer;

        private readonly int highlightId = Shader.PropertyToID("_Highlight");

        private void Reset()
        {
            renderer = GetComponent<Renderer>();
            SetupShader();
        }

        public void Highlight() => EnableHighlight(true);
        public void UnHighlight() => EnableHighlight(false);

        private void EnableHighlight(bool enabled)
        {
            var value = enabled ? 1F : 0F;
            renderer.material.SetFloat(highlightId, value);
        }

        private void SetupShader()
        {
            const string shaderName = "Shader Graphs/Lit Highlighter";
            var hasHighlightShader = renderer.sharedMaterial.shader.name == shaderName;

            if (hasHighlightShader) return;

            var shader = Shader.Find(shaderName);
            renderer.sharedMaterial.shader = shader;
        }
    }
}