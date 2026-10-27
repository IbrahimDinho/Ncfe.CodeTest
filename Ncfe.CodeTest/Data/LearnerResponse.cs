namespace Ncfe.CodeTest.Data
{
    public class LearnerResponse
    {
        /// <summary>
        /// Gets or sets whether the learner is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the learner that was loaded.
        /// </summary>
        public Learner Learner { get; set; }
    }
}
