namespace Client
{
    internal struct HasPath
    {
        public PathProvider Path;
        public int CurrentPathPointIndex;
        public float MovingSpeed;
        public float CompleteRadius;
        public bool IsTeleportToBeginPath;
        public bool IsGoToBack;
        public bool IsNotFaceToDirection;
    }
}