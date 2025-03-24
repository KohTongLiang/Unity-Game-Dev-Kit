# Simple Save System

---

## Merge Strategies

---

The merging of Save Data is handled by Merge Strategies. Implement IMergeable<T> interface.

```C#
public class PlayerStats : IMergeable<PlayerStats>
{
    public int highScore;
    public int totalCoins;

    public PlayerStats MergeWith(PlayerStats other)
    {
        return new PlayerStats
        {
            highScore = Mathf.Max(highScore, other.highScore),
            totalCoins = totalCoins + other.totalCoins
        };
    }
}
```

## Examples

---

### Saving using Default Merge Strategies

```C#
var saveSystem = new PlayerPrefsSaveSystem();
var stats = new PlayerStats { highScore = 100, totalCoins = 50 };
saveSystem.Save("playerStats", stats);
```

### Saving using Dictionary Merge

```C#
var inventory = new Dictionary<string, int> { ["sword"] = 1, ["shield"] = 1 };
saveSystem.Save("inventory", inventory, MergePolicy.MergeDictionaries);
```

### Custom Merge Strategy

```C#
saveSystem.Save("quests", newQuests, MergePolicy.Custom, (existing, new) => {
    foreach (var quest in new)
    {
        if (!existing.ContainsKey(quest.Key))
        {
            existing.Add(quest.Key, quest.Value);
        }
    }
    return existing;
});
```