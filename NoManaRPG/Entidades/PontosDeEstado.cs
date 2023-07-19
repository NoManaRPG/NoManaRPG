// This file is part of NoManaRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades;

[BsonIgnoreExtraElements]
public class PontosDeEstado
{
    public double ValorMaximo { get; private set; }
    public double ValorAtual { get; private set; }

    public PontosDeEstado(double valorbBase)
    {
        this.ValorMaximo = valorbBase;
        this.ValorAtual = valorbBase;
    }

    public void ResetCurrentToMax() => this.ValorAtual = this.ValorMaximo;

    public void AddCurrent(double value)
    {
        this.ValorAtual += value;
        if (this.ValorAtual >= this.ValorMaximo)
            this.ValorAtual = this.ValorMaximo;
    }

    public bool RemoveCurrent(double value)
    {
        this.ValorAtual -= value;
        if (this.ValorAtual <= 0)
            return true;
        return false;
    }

    public void ChangeMaxValue(double value)
    {
        if (value < this.ValorAtual)
            this.ValorAtual = value;
        this.ValorMaximo = value;
    }
}
