List<(string non_terminal, List<(string terminal, List<string> t_productions)> nt_productions)> LTparsing_table = 
    new List<(string non_terminal, List<(string terminal, List<string> t_productions)> nt_productions)>()
{
    ("<if_stmt>", new List<(string terminal, List<string> t_productions)> 
    {
        ("if", new List<string> { "if <expr> then <stmt> <else_if_part> <else_part>" })
    }),
    
    ("<else_if_part>", new List<(string terminal, List<string> t_productions)>
    {
        ("else", new List<string> { "else_if <expr> then <stmt> <else_if_part>", "ε" }),
        ("ε", new List<string> { "ε" })
    }),
    
    ("<else_part>", new List<(string terminal, List<string> t_productions)>
    {
        ("else", new List<string> { "else <stmt>" }),
        ("ε", new List<string> { "ε" })
    }),
    
    ("<expr>", new List<(string terminal, List<string> t_productions)>
    {
        ("ID", new List<string> { "ID" }),
        ("NUM", new List<string> { "NUM" })
    }),
    
    ("<stmt>", new List<(string terminal, List<string> t_productions)>
    {
        ("ID", new List<string> { "ID" }),
        ("NUM", new List<string> { "NUM" })
    })
};


Console.WriteLine("Escriba su input para la gramatica separado por espacios");
string Sinput = Console.ReadLine();
string help = "<if_stmt>,";
string[] parts= Array.ConvertAll(Sinput.Split(' '), p => p.Trim());

System.Console.WriteLine(parts[0]);

Stack<string> SparsingStack = new Stack<string>();
SparsingStack.Push("eof");
SparsingStack.Push("<if_stmt>");

while(SparsingStack.Count > 0){
    string top = SparsingStack.Pop();

    if(parts.Length == 0){
        parts = parts.Append("ε").ToArray();
    }

    if(top.StartsWith("<") && top.EndsWith(">")){
        var f_non_terminal = LTparsing_table.FirstOrDefault(nt => nt.non_terminal == top);
        List<(string terminal, List<string> t_productions)> non_terminal_productions = f_non_terminal.nt_productions;

        var f_terminal = non_terminal_productions.FirstOrDefault(nt => nt.terminal == parts[0]);
        List<string> terminal_productions = f_terminal.t_productions;

        if(terminal_productions != null){
            int Iindex = -1;
            for(int i = 0; i < terminal_productions.Count; i++){
                if(terminal_productions[i].Split(' ')[0] == parts[0] || terminal_productions[i] == "ε"){
                    Iindex = i;
                    break;
                }
            }

        if (terminal_productions[Iindex] != "ε"){
            var productionToPush = terminal_productions[Iindex].Split(' ');
                for (int i = productionToPush.Length - 1; i >= 0; i--){
                    SparsingStack.Push(productionToPush[i]);
                }
            }
        }
        else break;
    }
    else if (top == parts[0]){
        parts = parts.Skip(1).ToArray();
    }
    else{
        break;
    }
}
if(parts[0] == "ε" && SparsingStack.Count == 0) System.Console.WriteLine("CADENA ACEPTADA");
else System.Console.WriteLine("CADENA NO ACEPTADA");