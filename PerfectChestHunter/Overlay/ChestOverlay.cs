using Il2Cpp;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PerfectChestHunter.Overlay;

/// <summary>
/// Puts a text label next to every unopened chest showing what is inside. Only reads game state,
/// never opens anything. Labels are clones of the chest's own reveal text, so they use the game's font.
/// </summary>
internal sealed class ChestOverlay(ChestHunt hunt, Settings settings)
{
    private const string LabelObjectName = "PerfectChestHunter_Overlay";

    private sealed class Label
    {
        public TextMeshProUGUI Text; // null when no label could be created for this chest
        public ChestLabel? Shown;
    }

    private readonly Dictionary<int, Label> _labels = new();
    private readonly HashSet<int> _visible = new();

    internal void Refresh()
    {
        _visible.Clear();

        var chests = hunt.Chests;
        var best = ChestPicks.BestMultiplier(chests);
        foreach (var chest in chests)
        {
            if (!ChestHunt.IsOpenable(chest)) continue;
            var obj = chest.chestObject;
            Show(obj, obj.revealText, ChestLabel.For(chest, ReferenceEquals(chest, best)));
        }

        // Perfect-chest stage: label the choices until one of them is picked.
        var perfectChests = hunt.PerfectChests;
        if (!perfectChests.Any(c => c != null && c.opened))
        {
            var pick = ChestPicks.BestPerfectChest(perfectChests);
            foreach (var chest in perfectChests)
            {
                var obj = chest?.perfectChestObject;
                if (obj == null) continue;
                Show(obj, obj.revealText, ChestLabel.For(chest, ReferenceEquals(chest, pick)));
            }
        }

        RemoveHiddenLabels();
    }

    internal void Clear()
    {
        if (_labels.Count == 0) return;
        foreach (var label in _labels.Values)
            Destroy(label);
        _labels.Clear();
    }

    private void Show(Component owner, TextMeshProUGUI template, ChestLabel content)
    {
        var id = owner.GetInstanceID();
        _visible.Add(id);

        if (!_labels.TryGetValue(id, out var label))
        {
            label = new Label { Text = CreateLabel(owner.transform, template) };
            _labels[id] = label;
        }

        if (label.Text == null || label.Shown == content) return;

        label.Text.text = content.Text;
        label.Text.color = content.Color;
        label.Shown = content;
    }

    private void RemoveHiddenLabels()
    {
        if (_labels.Count == _visible.Count) return;

        foreach (var id in _labels.Keys.Where(id => !_visible.Contains(id)).ToList())
        {
            Destroy(_labels[id]);
            _labels.Remove(id);
        }
    }

    private TextMeshProUGUI CreateLabel(Transform parent, TextMeshProUGUI template)
    {
        if (template == null)
            template = parent.GetComponentInChildren<TextMeshProUGUI>(true);

        if (template == null || template.gameObject == parent.gameObject || template.name == LabelObjectName)
        {
            Plugin.LogDebug($"No text to clone on {parent.name}; chest left without overlay label");
            return null;
        }

        var go = UnityEngine.Object.Instantiate(template.gameObject, parent, false);
        go.name = LabelObjectName;

        // The template is animated by the chest; the copy must not be.
        var animator = go.GetComponent<Animator>();
        if (animator != null) UnityEngine.Object.Destroy(animator);
        go.AddComponent<LayoutElement>().ignoreLayout = true;

        var text = go.GetComponent<TextMeshProUGUI>();
        text.raycastTarget = false; // never block clicks on the chest
        text.richText = true;
        text.enableAutoSizing = false;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;
        text.alignment = TextAlignmentOptions.MidlineLeft;
        if (settings.OverlayFontSize.Value > 0)
            text.fontSize = settings.OverlayFontSize.Value;

        // Anchor to the right edge of the chest icon, vertically centred.
        var rect = text.rectTransform;
        rect.anchorMin = rect.anchorMax = new Vector2(1f, 0.5f);
        rect.pivot = new Vector2(0f, 0.5f);
        rect.anchoredPosition = new Vector2(settings.OverlayOffsetX.Value, settings.OverlayOffsetY.Value);
        rect.sizeDelta = new Vector2(200f, 40f);
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        go.transform.SetAsLastSibling();
        go.SetActive(true);
        return text;
    }

    private static void Destroy(Label label)
    {
        if (label.Text != null)
            UnityEngine.Object.Destroy(label.Text.gameObject);
    }
}
