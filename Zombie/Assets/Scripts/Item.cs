using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    public enum Types
    {
        Coin,
        Ammo,
        Health,
    }

    public Types itemType;
    public int value = 10;
    public void Use(GameObject go)
    {
        switch (itemType)
        {
            case Types.Coin:
                break;
            case Types.Ammo:
                {
                    var shooter = go.GetComponent<PlayerShooter>();
                    shooter?.gun?.AddAmmo(value);
                }
                break;
            case Types.Health:
                {
                    var playerHealth = go.GetComponent<PlayerHealth>();
                    playerHealth?.Heal(value);
                }
                break;
            default:
                break;
        }
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Use(other.gameObject);
        }
    }
}