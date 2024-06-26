namespace Items
{
    internal interface ICarryable
    {
        public void PickUp(Photon.Realtime.Player player);
        public void Drop();
    }
}
