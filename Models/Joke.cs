namespace JokesWebApp.Models
{
    // Classe Joke
    public class Joke
    {
        //SETTER's & GETTER's
        public int id { get; set; }
        public string JokeQuestion { get; set; }
        public string JokeAnswer { get; set; }

        // Costruttore
        public Joke()
        {
            
        }

    }
}
