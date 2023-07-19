// This file is part of NoManaRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;
using NoManaRPG.Extensions;

namespace NoManaRPG.Entidades;

[BsonIgnoreExtraElements]
public class Personagem
{
    public PontosDeEstado PontosDeVida { get; private set; }
    public PontosDeEstado PontosDeMana { get; private set; }
    public PontosDeEstado EstaminaMental { get; private set; }
    public PontosDeEstado EstaminaFisica { get; private set; }
    public PontosDeEstado Sono { get; private set; }
    public PontosDeEstado Fome { get; private set; }
    public PontosDeEstado Sede { get; private set; }

    public Atributo Forca { get; private set; }
    public Atributo Destreza { get; private set; }
    public Atributo Constituicao { get; private set; }
    public Atributo Inteligencia { get; private set; }
    public Atributo Agilidade { get; private set; }
    public Atributo Carisma { get; private set; }

    public double NivelDeCombate => this.Forca + this.Destreza + this.Constituicao + this.Inteligencia + this.Agilidade + this.Carisma;

    public Personagem()
    {
        Random rand = new ();
        this.Forca = new(rand.Sortear(1, 10));
        this.Destreza = new(rand.Sortear(1, 10));
        this.Constituicao = new(rand.Sortear(1, 10));
        this.Inteligencia = new(rand.Sortear(1, 10));
        this.Agilidade = new(rand.Sortear(1, 10));
        this.Carisma = new(rand.Sortear(1, 10));

        this.EstaminaMental = new((this.Inteligencia + this.Carisma) * 5);
        this.EstaminaFisica = new((this.Forca + this.Constituicao) * 5);

        this.PontosDeVida = new(this.Forca * 100);
        this.PontosDeMana = new((this.EstaminaMental.ValorAtual + this.Constituicao.ValorAtual) / 8);

        this.Sono = new(this.Constituicao * 100);
        this.Fome = new(this.Constituicao * 10);
        this.Sono = new(this.Constituicao * 10);
    }
}
