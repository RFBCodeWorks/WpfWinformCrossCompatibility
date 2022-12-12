using System.Threading.Tasks;

namespace CustomControls.Interfaces
{
    /// <summary>
    /// Interface for interacting with a control that controls a RoboSharp object
    /// </summary>
    public interface IRoboQueueControl
    {
        /// <summary>
        /// Flag to tell if the copy operation has been cancelled
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        /// Flag to tell if the copy operation has completed running
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Flag to tell if the copy operation is currently paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// Flag to tell if the copy operation is currently running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Flag to tell if the copy operation has been started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Close the window
        /// </summary>
        void Close();

        /// <summary>
        /// Hide the control / form <para/>
        /// </summary>
        void Hide();

        /// <summary>
        /// Show the control / form <para/>
        /// </summary>
        void Show();

        /// <summary>
        /// Wait until the copy operations have finished running <br/>
        /// Waiting thread will check every 250ms until the tasks are complete or cancelled.
        /// </summary>
        /// <returns></returns>
        Task WaitUntilComplete();

        /// <summary>
        /// Start the RoboQueue process
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the RoboQueue process
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause the RoboQueue process
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume the RoboQueue process
        /// </summary>
        void Resume();
    }
}