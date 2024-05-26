using Unity.VisualScripting;

namespace Logan.Helpers
{
    public class Countdown
    {
        public Countdown(int counter)
        {
            initialCounter = counter + 1;
        }
        private readonly int initialCounter;
        private int counter;
        private double timeSinceLastSecond;
        public bool Enabled {  get; private set; }
        public int Value => counter; 
        
        public void Start()
        {
            Enabled = true;
            timeSinceLastSecond = 0;
            counter = initialCounter;
        }

        public bool CanTick(float dt)
        {
            if (!Enabled)
            {
                return false;
            }
            timeSinceLastSecond += dt;
            if (timeSinceLastSecond >= 1 || counter == initialCounter)
            {
                timeSinceLastSecond = 0;
                counter--;
                return true;
            }

            if (counter == 0)
            {
                Enabled = false;
            }

            return false;
        }
    }
}