namespace API.Helpers
{
    public class UserParams : PaginationParams
    {   

        public string CurrentUsername { get; set; }
        
        public string Position { get; set; }    
        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 50;

        public string OrderBy { get; set; } = "lastActive"; 
        

    }
}