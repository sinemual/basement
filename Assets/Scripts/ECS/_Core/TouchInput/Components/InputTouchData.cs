using UnityEngine;

namespace Client
{
    internal struct InputTouchData
    {
        public Touch CurrentTouch;
        public TouchPhase TouchPhase;
        public Vector2 MoveTouchStartPosition;
    }
}