using System;
using System.Collections.Generic;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Premix.Models
{
    public class Tarefa : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public ObjectId Id { get; set; }  
        
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("UsuarioId")]
        public ObjectId UsuarioId { get; set; } // Quem criou a tarefa (chefe logado)

        private string _nome = string.Empty;
        private DateTime _vencimento;      
        private string _descricao = string.Empty;
        private bool _status;              // 🔹 true = Andamento, false = Concluído
        
        [BsonElement("departamento")]
        public string Departamento { get; set; }

        [BsonElement("funcionario")]
        public string Funcionario { get; set; }
        
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

        [BsonElement("status")]
        public bool Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(StatusTexto));
                }
            }
        }

        [BsonIgnore]
        public string StatusTexto => Status ? "Andamento" : "Concluído";

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
