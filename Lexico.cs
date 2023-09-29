using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace lexii2
{
    public class Lexico : Token, IDisposable
    {
        private StreamReader archivo;
        private StreamWriter log;
        public Lexico()
        {
            archivo = new StreamReader("prueba.cpp");
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            const int F = -1;
            const int E = -2;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();
                switch (estado)
                {
                    case 0:
                        if (char.IsWhiteSpace(c))
                            estado = 0;
                        else if (char.IsLetter(c))
                            estado = 1;
                        else if (char.IsDigit(c))
                            estado = 2;
                        else if (c == '=')
                            estado = 8;
                        else if (c == '=')
                            estado = 9;
                        else if (c == ';')
                            estado = 10;
                        else if (c == '&')
                            estado = 11;
                        else if (c == '|')
                            estado = 12;
                        else if (c == '!')
                            estado = 13;
                        else if (c == '&' || c == '|')
                            estado = 14;
                        else if (c == '=')
                            estado = 15;
                        else if (c == '>')
                            estado = 16;
                        else if (c == '<')
                            estado = 17;
                        else if (c == '=' || c == '>')
                            estado = 18;
                        else if (c == '+')
                            estado = 19;
                        else if (c == '-')
                            estado = 20;
                        else if (c == '*' || c == '/' || c == '%')
                            estado = 22;
                        else if (c == '=')
                            estado = 23;
                        else if (c == '?')
                            estado = 24;
                        else if (c == '"')
                            estado = 25;   //////
                        else if (c == '"')
                            estado = 26;    //////
                        else
                            estado = 27;
                        break;
                    case 1:
                        setClasificacion(Tipos.Identificador);
                        if (!char.IsLetterOrDigit(c))
                            estado = F;
                        break;
                    case 2:
                        setClasificacion(Tipos.Numero);
                        if (char.IsDigit(c))
                            estado = 2;
                        else if (c == '.')
                            estado = 3;
                        else if (char.ToLower(c) == 'e')
                            estado = 5;
                        else
                            estado = F;
                        break;
                    case 3:
                        if (char.IsDigit(c))
                            estado = 4;
                        else
                            estado = E;
                        break;
                    case 4:
                        if (char.IsDigit(c))
                            estado = 4;
                        else if (char.ToLower(c) == 'e')
                            estado = 5;
                        else
                            estado = F;
                        break;
                    case 5:
                        if (char.IsDigit(c))
                            estado = 7;
                        else if (c == '+' || c == '-')
                            estado = 6;
                        else
                            estado = E;
                        break;
                    case 6:
                        if (char.IsDigit(c))
                            estado = 7;
                        else
                            estado = E;
                        break;
                    case 7:
                        if (!char.IsDigit(c))
                            estado = F;
                        break;
                    case 8:
                        setClasificacion(Tipos.Asignacion);
                        if (c == '=')
                            estado = 9;
                        else
                            estado = F;
                        break;
                    case 9:
                        setClasificificacion(Tipos.OpRelacional);
                        estado = F;
                        break;
                    case 10:
                        setClasificificacion(Tipos.FindeSentencia);
                        estado = F;
                        break;
                    case 11:
                        setClasificificacion(Tipos.Caracter);
                        if (c == '&')
                            estado = 14;
                        else
                            estado = F;
                        break;
                    case 12:
                        setClasificificacion(Tipos.Caracter);
                        if (c == '|')
                            estado = 14;
                        else
                            estado = F;
                    case 13:
                        setClasificificacion(Tipos.OpLogico);
                        if (c == '=')
                            estado = 15;
                        else
                            estado = F;
                    case 14:
                        setClasificificacion(Tipos.OpLogico);
                        estado = F;
                    case 15:
                        setClasificificacion(Tipos.OpRelacional);
                        estado = F;
                    case 16:
                        setClasificificacion(Tipos.OpRelacional);
                        if (c == '=')
                            estado = 18;
                        else
                            estado = F;
                    case 17:
                        setClasificificacion(Tipos.OpRelacional);
                        if (c == '>' || c == '=')
                            estado = 18;
                        else
                            estado = F;
                    case 18:
                        setClasificificacion(Tipos.OpRelacional);
                        estado = F;
                    case 19:
                        setClasificificacion(Tipos.OpTermino);
                        if (c == '=' || c == '+')
                            estado = 21;
                        else
                            estado = F;
                    case 20:
                        setClasificificacion(Tipos.OpTermino);
                        if (c == '=' || c == '-')
                            estado = 21;
                        else
                            estado = F;
                    case 21:
                        setClasificificacion(Tipos.IncTermino);
                        estado = F;
                    case 22:
                        setClasificificacion(Tipos.OpFactor);
                        if (c == '=')
                            estado = 23;
                        else
                            estado = F;
                    case 23:
                        setClasificificacion(Tipos.OpFactor);
                        estado = F;
                    case 24:
                        setClasificificacion(Tipos.OpTernario);
                        estado = F;
                    case 25:
                        setClasificificacion(Tipos.Cadena);
                        if (c == '"')
                            estado = 26;
                        else if (FinArchivo())
                            estado = E;
                    case 26:
                        setClasificificacion(Tipos.Cadena);
                        estado = F;
                    case 27:
                        setClasificificacion(Tipos.Caracter);
                        estado = F;
                }
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    archivo.Read();
                }
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + " = " + getClasificacion());
        }
    }
    public bool FinArchivo()
    {
        return archivo.EndOfStream;
    }
 }