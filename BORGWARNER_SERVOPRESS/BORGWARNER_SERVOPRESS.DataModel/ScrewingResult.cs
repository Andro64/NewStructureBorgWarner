namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class ScrewingResult
    {
        public bool status { get; set; }
        public bool timeout { get; set; }
        public bool canceled_by_user { get; set; }
    }
}
