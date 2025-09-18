using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Premix.Models;

public class Departamentos : INotifyPropertyChanged
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("UsuarioId")]
    public string UsuarioId { get; set; }   // <-- só essa, sem o campo privado

    private string _nome = string.Empty;
    private string _codigo = string.Empty;
    private bool _isActive;

    [BsonElement("nome")]
    public string Nome
    {
        get => _nome;
        set
        {
            if (_nome != value)
            {
                _nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }
    }

    [BsonElement("codigo")]
    public string Codigo
    {
        get => _codigo;
        set
        {
            if (_codigo != value)
            {
                _codigo = value;
                OnPropertyChanged(nameof(Codigo));
            }
        }
    }

    [BsonElement("isActive")]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    [BsonIgnore]
    public string Status => IsActive ? "Ativo" : "Inativo";

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
    
