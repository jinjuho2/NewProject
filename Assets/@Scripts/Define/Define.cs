using UnityEngine;

namespace Define
{
    public enum Escene
    {
        None = -1,
        Title,
        Lobby,
        Game,
    }

    public enum ESavePopupResult
    {
        Slot0,
        Slot1,
        Slot2,
        Delete = 100,
        Cancel = 200,
    }
}
