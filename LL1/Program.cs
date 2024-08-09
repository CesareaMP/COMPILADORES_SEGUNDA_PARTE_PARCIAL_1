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

string Evaluate(string Sinput){
    string help = "<if_stmt>,";
    string[] parts= Array.ConvertAll(Sinput.Split(' '), p => p.Trim());

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
if(parts[0] == "ε" && SparsingStack.Count == 0) return "CADENA ACEPTADA";
else return "CADENA NO ACEPTADA";
}

System.Console.WriteLine("CADENA 1: if ID then ID");
System.Console.WriteLine(Evaluate("if ID then ID\n"));

System.Console.WriteLine("CADENA 2: if ID then ID else NUM");
System.Console.WriteLine(Evaluate("if ID then ID else NUM\n"));

System.Console.WriteLine("CADENA 3: if NUM then NUM");
System.Console.WriteLine(Evaluate("if NUM then NUM\n"));

System.Console.WriteLine("CADENA 4: if then ID");
System.Console.WriteLine(Evaluate("if then ID\n"));

System.Console.WriteLine("CADENA 5: if ID then ID else then");
System.Console.WriteLine(Evaluate("if ID then ID else then\n"));

System.Console.WriteLine("CADENA 6: if ID then ID else then");
System.Console.WriteLine(Evaluate("if ID then ID else then\n"));

System.Console.WriteLine("Ingrese su cadena separada por espacios");
string input = Console.ReadLine();
System.Console.WriteLine(Evaluate(input));