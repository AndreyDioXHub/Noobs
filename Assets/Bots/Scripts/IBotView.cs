
using UnityEngine;

namespace cyraxchel.ai.bots {

    public interface IBotView {
        bool PlayerInSightView(Vector3 botPosition , float sightRange, LayerMask whatIsPlayer);
        bool PlayerInSightView();
        bool CanAttackPlayer(Vector3 botPosition, float attackRange, LayerMask whatIsPlayer);
        bool CanAttackPlayer();
    }
}