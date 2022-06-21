using System.Threading.Tasks;
    public interface IRabbitMQService
    {
        Task SendUserAsync(User u);
    }