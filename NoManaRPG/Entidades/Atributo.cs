// This file is part of NoManaRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades;

[BsonIgnoreExtraElements]
public class Atributo
{
    public double ValorMaximo { get; private set; }
    public double ValorAtual { get; private set; }

    public double ValorBase { get; private set; }
    public double VezesTreinado { get; private set; }

    public Atributo(double valorBase)
    {
        this.ValorMaximo = valorBase;
        this.ValorAtual = valorBase;
        this.ValorBase = valorBase;
        this.VezesTreinado = 0;
    }

    public void ResetCurrentToMax() => this.ValorAtual = this.ValorMaximo;

    public void AddCurrent(double value)
    {
        this.ValorAtual += value;
        if (this.ValorAtual >= this.ValorMaximo)
            this.ValorAtual = this.ValorMaximo;
    }
    public void RemoveCurrent(double value) => this.ValorAtual -= value;

    public void AddMaximo(double value) => this.ValorMaximo += value;

    public void RemoveMaximo(double value)
    {
        this.ValorMaximo -= value;
        if (this.ValorAtual >= this.ValorMaximo)
            this.ValorAtual = this.ValorMaximo;
    }

    public void Treinar() => this.VezesTreinado++;

    public static double operator +(Atributo a, Atributo b) => a.ValorAtual + b.ValorAtual;
    public static double operator +(double a, Atributo b) => a + b.ValorAtual;
    public static double operator *(int a, Atributo b) => a * b.ValorAtual;
    public static double operator *(Atributo a, int b) => b * a.ValorAtual;
}
