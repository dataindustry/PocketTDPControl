namespace PocketTDPControl
{
    public class MainService : IMainService
    {
        public void Adjust(string target, int tdp)
        {
            Operation.Adjust(target, tdp);
        }

        public bool CheckCoupling() {
            return true;
        }

        public int GetReadingTDP() {
            return 0;
        }

        public int GetBatteryEstimatedRunTime() {
            return 0;
        }
    }
}
