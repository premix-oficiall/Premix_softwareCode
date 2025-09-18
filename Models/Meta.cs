using System;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Premix.Models;

public class Meta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; } 
    
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("UsuarioId")]
    public string UsuarioId { get; set; }   // <-- só essa, sem o campo privado
    
    private string _nome = string.Empty;
    private DateTime _vencimento = DateTime.Now;      
    private string _descricao = string.Empty;
    private int _progresso = 0;
    private string _recompensa = string.Empty;
    
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

    
    [BsonElement("vencimento")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Vencimento
    {
        get => _vencimento;
        set
        {
            if (_vencimento != value)
            {
                _vencimento = value;
                OnPropertyChanged(nameof(Vencimento));
            }
        }
    }

    
    [BsonElement("descricao")]
    public string Descricao
    {
        get => _descricao;
        set
        {
            if (_descricao != value)
            {
                _descricao = value;
                OnPropertyChanged(nameof(Descricao));
            }
        }
    }

    
    [BsonElement("progresso")]
    public int Progresso
    {
        get => _progresso;
        set
        {
            if (_progresso != value)
            {
                _progresso = value;
                OnPropertyChanged(nameof(Progresso));
            }
        }
    }

    [BsonElement("recompensa")]
    public string Recompensa
    {
        get => _recompensa;
        set
        {
            if (_recompensa != value)
            {
                _recompensa = value;
                OnPropertyChanged(nameof(Recompensa));
            }
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}