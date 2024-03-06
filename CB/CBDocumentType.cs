namespace ResidentialRegistration.CB
{
    internal class CBDocumentType
    {
        public long id { get; set; }

        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
