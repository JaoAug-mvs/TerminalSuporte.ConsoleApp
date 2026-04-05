// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;

internal static class Program
{
    private static readonly string[] ComandosDisponiveis =
    {
        "PING",
        "RESET",
        "FORMATAR",
        "HELP ou ?",
        "SAIR"
    };

    private static void Main()
    {
        ConfigurarConsole();

        bool sistemaAtivo = true;

        while (sistemaAtivo)
        {
            MostrarTelaPrincipal();

            Console.Write("Digite um comando: ");
            string comando = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(comando))
            {
                NotificarAlerta("Nenhum comando foi informado. Escolha uma opcao do menu.");
                AguardarRetornoAoMenu();
                continue;
            }

            switch (comando)
            {
                case "PING":
                    ExecutarPing();
                    break;

                case "RESET":
                    ExecutarReset();
                    break;

                case "FORMATAR":
                    ExecutarFormatacao();
                    break;

                case "HELP":
                case "?":
                    MostrarAjuda();
                    break;

                case "SAIR":
                    sistemaAtivo = false;
                    break;

                default:
                    NotificarErro("Comando nao reconhecido. Use HELP para ver os comandos disponiveis.");
                    AguardarRetornoAoMenu();
                    break;
            }
        }

        LimparTela();
        NotificarSucesso("Encerrando o console com seguranca. Ate logo!");
    }

    private static void MostrarTelaPrincipal()
    {
        LimparTela();
        EscreverLinhaColorida("==============================================", ConsoleColor.Cyan);
        EscreverLinhaColorida("      CONSOLE DE DIAGNOSTICO DE REDE", ConsoleColor.Cyan);
        EscreverLinhaColorida("==============================================", ConsoleColor.Cyan);
        Console.WriteLine("Bem-vindo! Escolha uma opcao para continuar.\n");

        MostrarMenu();
    }

    private static void MostrarMenu()
    {
        // Heuristica #6: menu fixo para reconhecimento em vez de recordacao.
        EscreverLinhaColorida("Comandos disponiveis:", ConsoleColor.White);

        foreach (string comando in ComandosDisponiveis)
        {
            Console.WriteLine($" - {comando}");
        }

        Console.WriteLine();
    }

    private static void ExecutarPing()
    {
        Console.Write("Informe o IP de destino (ou pressione ENTER para cancelar): ");
        string destino = (Console.ReadLine() ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(destino))
        {
            NotificarAlerta("Operacao cancelada. Nenhum IP foi informado.");
            AguardarRetornoAoMenu();
            return;
        }

        if (!IpValido(destino))
        {
            NotificarErro("IP invalido. Use o formato xxx.xxx.xxx.xxx, por exemplo: 192.168.0.10.");
            AguardarRetornoAoMenu();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Preparando envio de pacotes...");
        Thread.Sleep(600);
        Console.WriteLine($"Enviando pacotes para {destino}...");
        Thread.Sleep(900);

        int latencia = Random.Shared.Next(8, 85);
        NotificarSucesso($"Resposta recebida de {destino}: tempo={latencia}ms, pacotes=4, perda=0%");
        AguardarRetornoAoMenu();
    }

    private static void ExecutarReset()
    {
        Console.WriteLine();
        NotificarAlerta("ATENCAO: voce esta prestes a reiniciar o servidor.");

        // Heuristica #5: confirmacao explicita antes de acao critica.
        EscreverLinhaColorida("Essa acao pode interromper servicos temporariamente.", ConsoleColor.Red);

        if (!SolicitarConfirmacao("Deseja continuar com o RESET? (S/N): "))
        {
            NotificarAlerta("Operacao abortada com seguranca. Nenhuma alteracao foi realizada.");
            AguardarRetornoAoMenu();
            return;
        }

        Console.WriteLine("Iniciando reinicializacao controlada do servidor...");
        Thread.Sleep(900);
        Console.WriteLine("Encerrando servicos...");
        Thread.Sleep(900);
        Console.WriteLine("Reiniciando componentes...");
        Thread.Sleep(1000);
        NotificarSucesso("Servidor reiniciado com sucesso na simulacao.");
        AguardarRetornoAoMenu();
    }

    private static void ExecutarFormatacao()
    {
        Console.WriteLine();
        NotificarAlerta("RISCO MAXIMO: voce selecionou FORMATAR.");

        // Heuristica #5: alerta forte + confirmacao explicita em acao extremamente critica.
        EscreverLinhaColorida("Esta simulacao representa uma acao destrutiva e irreversivel.", ConsoleColor.Red);
        EscreverLinhaColorida("Nada sera executado sem a sua confirmacao explicita.", ConsoleColor.Red);

        if (!SolicitarConfirmacao("Confirma que deseja prosseguir com FORMATAR? (S/N): "))
        {
            NotificarAlerta("Formatacao cancelada. Retornando ao menu principal sem falhas.");
            AguardarRetornoAoMenu();
            return;
        }

        Console.Write("Para confirmar de forma explicita, digite FORMATAR novamente: ");
        string confirmacaoFinal = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

        if (!string.Equals(confirmacaoFinal, "FORMATAR", StringComparison.Ordinal))
        {
            NotificarAlerta("Confirmacao final nao reconhecida. Operacao cancelada com seguranca.");
            AguardarRetornoAoMenu();
            return;
        }

        Console.WriteLine("Preparando formatacao simulada...");
        Thread.Sleep(900);
        Console.WriteLine("Verificando unidade...");
        Thread.Sleep(900);
        Console.WriteLine("Executando procedimento simulado...");
        Thread.Sleep(1000);
        NotificarSucesso("Formatacao simulada concluida. Nenhum dado real foi alterado.");
        AguardarRetornoAoMenu();
    }

    private static void MostrarAjuda()
    {
        // Heuristica #10: ajuda acessivel sem encerrar o sistema.
        Console.WriteLine();
        EscreverLinhaColorida("AJUDA RAPIDA", ConsoleColor.Cyan);
        Console.WriteLine("PING      - Valida um IP e simula o envio de pacotes com latencia ficticia.");
        Console.WriteLine("RESET     - Simula a reinicializacao do servidor apos confirmacao S/N.");
        Console.WriteLine("FORMATAR  - Simula uma acao extremamente critica com dupla confirmacao.");
        Console.WriteLine("HELP      - Exibe esta ajuda resumida sem sair do sistema.");
        Console.WriteLine("?         - Atalho para o comando HELP.");
        Console.WriteLine("SAIR      - Encerra a aplicacao de forma amigavel.");
        Console.WriteLine();
        Console.WriteLine("Dica: se um comando for invalido, use HELP para consultar o menu novamente.");
        AguardarRetornoAoMenu();
    }

    private static void NotificarErro(string mensagem)
    {
        EscreverLinhaColorida($"[ERRO] {mensagem}", ConsoleColor.Red);
    }

    private static void NotificarSucesso(string mensagem)
    {
        EscreverLinhaColorida($"[OK] {mensagem}", ConsoleColor.Green);
    }

    private static void NotificarAlerta(string mensagem)
    {
        EscreverLinhaColorida($"[ATENCAO] {mensagem}", ConsoleColor.Yellow);
    }

    private static bool IpValido(string ipInformado)
    {
        return IPAddress.TryParse(ipInformado, out IPAddress? endereco)
            && endereco.AddressFamily == AddressFamily.InterNetwork;
    }

    private static bool SolicitarConfirmacao(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string resposta = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            if (resposta == "S")
            {
                return true;
            }

            if (resposta == "N")
            {
                return false;
            }

            NotificarErro("Resposta invalida. Digite S para sim ou N para nao.");
        }
    }

    private static void AguardarRetornoAoMenu()
    {
        Console.WriteLine();
        Console.Write("Pressione ENTER para voltar ao menu principal...");
        Console.ReadLine();
    }

    private static void ConfigurarConsole()
    {
        try
        {
            Console.Title = "TerminalSuporte.ConsoleApp";
        }
        catch (IOException)
        {
        }
    }

    private static void LimparTela()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            Console.WriteLine();
        }
    }

    private static void EscreverLinhaColorida(string mensagem, ConsoleColor cor)
    {
        try
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }
        catch (IOException)
        {
            Console.WriteLine(mensagem);
        }
    }
}
