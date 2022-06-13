<h1 align="center">JdSharp</h1>

<h3 align="center">JdSharp é um descompilador multiplataforma e multibackend escrita em C# na plataforma .NET</h3>

---
## Como funciona?

O JdSharp.Core funciona como uma API entre implementações de descompiladores e frontends, seguindo o padrão de 
aplicações como o [libretro](https://www.libretro.com/index.php/api/) faz com emuladores de consoles, possibilitando que através da interface 
`IDecompiler` e com os métodos utilitários disponibilizados sejá possivel implemetar a sua própria definição e lógica de descompilação, como 
é possivel ser possivel ser vista nas implementações do JdSharp.Cli e JdSharp.Web.

Definição da interface base de descompilação:

```csharp
  public interface IDecompiler
  {
      public IEnumerable<byte[]> AllowedFileSignatures { get; }
      public string FileExtension();

      DecompilerResult Decompile(DecompilerOptions options);
  }
```

## Implementação e fluxo do descompilador Java

### Leitura e Processamento

O nome JdSharp vem dos projetos originados a partir do core de descompilação para java chamado [jd-core](https://github.com/java-decompiler/jd-core), 
originalmente o projeto JdSharp era apenas um descompilador Java, porem foi possibilitado que o descompilador fosse multibackend mas o
nome continua por fazer parte da ideia original.

O processo de descompilação do Java é originalmente classificado pelos possiveis executáveis gerados pela Maquina Virtual Java (JVM),
sendo eles:
 - nomeDoArquivo.class
 - nomeDoArquivo.jar
 - nomeDoArquivo.war

Os arquivos de extensao `jar` e `war` não são nada mais que arquivos compactados com a estrutura do projeto Java definida internamente,
composta por varios arquivos de extensão `class` contendo o bytecode que é executado pelo Ambiente de Execução do Java (JRE). </br>
Os arquivos contendo o bytecode seguem uma estrutura interna que pode ser encontrada na [documentação da Oracle](https://docs.oracle.com/javase/specs/jvms/se7/html/jvms-4.html), seguindo a especificação e definição da estrutura da ClasFile.

Estrutura da ClassFile:

```c
ClassFile {
    u4             magic;
    u2             minor_version;
    u2             major_version;
    u2             constant_pool_count;
    cp_info        constant_pool[constant_pool_count-1];
    u2             access_flags;
    u2             this_class;
    u2             super_class;
    u2             interfaces_count;
    u2             interfaces[interfaces_count];
    u2             fields_count;
    field_info     fields[fields_count];
    u2             methods_count;
    method_info    methods[methods_count];
    u2             attributes_count;
    attribute_info attributes[attributes_count];
}
```

Através do `BigEndianessBinaryReader`, por conta que o `javac` apenas suporta compilação de arquivos com alta extremidade, é 
possivel realizar o processo de leitura binária sequencial a partir da definição da documentação, sendo salvo em uma classe que 
possui as mesmas propriedades equivalentes no C#, chamada de `JavaClassFile`.

### Tokenização

A partir da instanciação da classe JavaClassFile, com todos os valores presentes da leitura binária, é realizado o processo de 
tokenização dos valores presentes no arquivo Java, uma classe java segue uma estrutura padrão entre classes, interfaces e enumeraveis:

Estrutura padrão da classe Java:

```
  <access_flags> <this_class> [extends <super_class>] [implements <interfaces[interfaces_count]>] {
      <acess_flags> field_info;
      
      <acess_flags> <name_index> method_info(<constant_pool[descriptor_index]>) {
      } 
  }
```

As principais informações da classe Java contem os nomes originais contidos no código fonte, sendo possivel ainda obter as informações
originais da classe Java. A tokenização se possibilita destas informações para montar o código fonte da classe, utilizando as estruturas
de dados do `StringBuilder` para evitar problemas de re-alocação de memória.

Exemplo de tradução da [tabela](https://docs.oracle.com/javase/specs/jvms/se7/html/jvms-4.html#jvms-4.3.2) de `field descriptor`:

```csharp
  public static string FieldDescriptorToJava(string descriptor)
  {
      return descriptor switch
      {
          "D" => "double",
          "B" => "byte",
          "F" => "float",
          "I" => "int",
          "S" => "short",
          "Z" => "Boolean",
          "C" => "char",
          "V" => "void",
          _ => string.Empty
      };
  }
```

Com isso é possivel retornar um array de bytes para que qualquer implemtação trate o resultado independente da plataforma.

## Como rodar a aplicação ?

dependendo do seu terminal, use o `build.ps1` ou `build.sh`, ao executar será gerado um uma pasta do jdsharp em modo de Release, 
contendo o EXE ou ELF dependendo do seu sistema operacional. Para rodar use

```powershell
 .\jdsharp.exe --help
```

## Agradecimentos

Quero agradecer principalmente aos integrantes do grupo de TCC, sendo eles:
 - Lucas Henrique Chagas Carvalho 
 - Murilo Carlos Costa de Moura 
 - Rafael de Carvalho Suckert

Aos professor Maurício Asenjo e minha familia e dos integrantes do grupo, que apoiaram o projeto.

## Licença

O projeto está sob a licença Apache 2.0, disponibilizando uso e redistribuição livre a partir das especificações.
