﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
    public class Day10
    {
        private HashSet<char> openingChunk = new HashSet<char> { '(', '[', '{', '<' };
        private HashSet<char> closingChunk = new HashSet<char> { ')', ']', '}', '>' };
        private Dictionary<char, char> mapOpenToCloseChunkChar = new Dictionary<char, char>();
        private Dictionary<char, char> mapCloseToOpenChunkChar = new Dictionary<char, char>();

        private Dictionary<char, int> mapCloseErrorValue = new Dictionary<char, int>();
        private Dictionary<char, int> mapPartTwoValue = new Dictionary<char, int>();
        private List<string> incompleteLines = new List<string>();

        public void GetResult()
        {
            PopulateDataStructures();
            GetPartOne();
            GetPartTwo();
        }

        private void PopulateDataStructures()
        {
            mapOpenToCloseChunkChar['('] = ')';
            mapOpenToCloseChunkChar['['] = ']';
            mapOpenToCloseChunkChar['{'] = '}';
            mapOpenToCloseChunkChar['<'] = '>';

            mapCloseToOpenChunkChar[')'] = '(';
            mapCloseToOpenChunkChar[']'] = '[';
            mapCloseToOpenChunkChar['}'] = '{';
            mapCloseToOpenChunkChar['>'] = '<';

            mapCloseErrorValue[')'] = 3;
            mapCloseErrorValue[']'] = 57;
            mapCloseErrorValue['}'] = 1197;
            mapCloseErrorValue['>'] = 25137;

            mapPartTwoValue['('] = 1;
            mapPartTwoValue['['] = 2;
            mapPartTwoValue['{'] = 3;
            mapPartTwoValue['<'] = 4;

        }

        private void GetPartOne()
        {
            var rows = startingInput.Split(new string[] { "\r\n" },
                                                   StringSplitOptions.RemoveEmptyEntries);

            long finalResult = 0;
            foreach (var row in rows)
            {
                bool isLineCorrupted = false;
                var openingChunckChars = new List<char>();
                foreach (var element in row)
                {
                    //We're opening the chunks
                    if (openingChunk.Contains(element))
                    {
                        openingChunckChars.Add(element);
                    }
                    else
                    {
                        //The chunk is closing, we have to check if it's the correct token

                        var lastElementOpeningChunks = openingChunckChars.LastOrDefault();

                        if (mapOpenToCloseChunkChar[lastElementOpeningChunks] == element)
                        {
                            openingChunckChars.RemoveAt(openingChunckChars.Count - 1);
                        }
                        else
                        {
                            finalResult += mapCloseErrorValue[element];
                            isLineCorrupted = true;
                            break;
                        }
                    }
                }

                if (!isLineCorrupted)
                    incompleteLines.Add(row);
            }

        }

        private void GetPartTwo()
        {
            List<double> resultList = new List<double>();
            foreach (var row in incompleteLines)
            {
                var openingChunckChars = new List<char>();
                foreach (var element in row)
                {
                    //We're opening the chunks
                    if (openingChunk.Contains(element))
                    {
                        openingChunckChars.Add(element);
                    }
                    else
                    {
                        //The chunk is closing, we have to check if it's the correct token

                        var lastElementOpeningChunks = openingChunckChars.LastOrDefault();

                        if (mapOpenToCloseChunkChar[lastElementOpeningChunks] == element)
                        {
                            openingChunckChars.RemoveAt(openingChunckChars.Count - 1);
                        }
                        else
                        {
                            throw new Exception("shouldn't go never here in my mind.");
                        }
                    }
                }


                //Now remains only the left opened tokens
                openingChunckChars.Reverse();

                double resultRow = 0;
                foreach (var openingChar in openingChunckChars)
                {
                    resultRow = resultRow * 5 + mapPartTwoValue[openingChar];
                }

                resultList.Add(resultRow);
            }

            var orderedList = resultList.OrderBy(i => i);

            var indexNeeded = (orderedList.Count() - 1) / 2;

            var finalResult = orderedList.ElementAt(indexNeeded);

        }

        #region input


        string startingInputTest =
  @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";



        string startingInput =
  @"[{<(<[<<(<{[[<[]{}><()[]>]]<(([]<>)<(){}>)({[]{}}([]()))>}>{{{<(()<>)[[]()]><(<>())[[]<>]>}{({()
<<[{([[{{[[[({()()}[<>[]]){<<>[]>([]<>)}]{[{{}{}}<{}{}>]{{[]<>}{[]{}}}}][{{{(){}}<<>[]>}}]]}{
([<{([<[([({[{[]{}}{{}{}}]}{[{<>{}}{(){}}]})[{[[(){}][(){}]]{<<>[]}{{}[]}}}]])]><({{{[[{{}{}}]<{<>
{{((<([<[<[((<{}>{()()})<[{}[]]{<>{}}>)<({[]{}}([][])){{[]<>}{{}<>}}>]>]><[[[[[<<>()>(()[])](<[][]>{<>{}}
{[[<[[((<[{{<[[]{}](()[])>}{<({}[])[[]<>]>}}{[(<()()><()>)({[]()}[<>()>)]}]((<{[<>[]]{{}()}}>[{<()<>>{[]
({{((<[{{{([{[(){}][<>]}([<>()][[]()])]{<{{}<>}<{}<>>>{{()[]}{()()}}})<<[(<><>)<[]()>][((){}){[]{}}]>>}}
{<(<{[[([[(<[(<>[])]<{()}<()[]>>><{<{}[]><[]>}(<<>{}><()()>)>)]]{((({{<><>}{{}{}}}{<[]()><{}<>>})<[{(
<[{(<{<((<(<{(<>{})}<<[]>(<>{})>>){{(<()[]>[[]<>])}[{[[][]]<<>()>}{[{}()]<(){}>}]}>{(((<<>
(<[{[<((((<[[[{}{}]{{}<>}]({[][]}[(){}])][{(<>[])<[]<>>}<[(){}]>]>)))<({{[<([]<>)<()<>>>[{
{{<<[<({[{([(<[][]>{[][]})<<<>[]>([]())>])[(<<[]<>>[{}()]>{([]{})})]}](<[<(<<>[]>)>{(<[]{}>[<>[]])<<[]()>((
[[[{<{[[[([[<<<>>[{}()]}[({}[])<()<>>]]]{<<<[]{}>[<>[]]><{[]<>}{[]{}}>>{(<{}{}>{<><>})({[]()}{{}<>})}})]<<<[(
(({([<{{{{<<<({}())[()<>]>{[[]()][()[]}}>>}<({({{}<>}[[]{}])(({}{}){()()})}{[(<>{}){<>{}}]{[<>[]]({}<>)}})>}(
{(([{(([<[{(<{[]<>}<()<>>>[([]{}}(<>{})])}({[<<>[]><<>{}>]}<[<()[]>[<>{}]]{<()<>>{(){}}}>)]>([<((<{}()>
({{<{{{{{{({<{<>{}}>(<<>[]>[[][]])})]{[{{{(){}}[()<>]}<[<>][[]{}]>}{[<{}<>>{<>}]{{[]{}}(<>{})
{{(([{([{([<([[]{}]{<>[]})>{(((){})[()()]){{{}<>]<()()>}}])(({{{<>{}}{[]{}}}[((){})[<>]]}<{(()[])[
{<[((<<(([{{<{[][]}>{[()<>]{()[]}}}}]))<<((<[<[][]>[[]()]][({}){[]<>}]>))>>>>))<(<<([<<[{<<><>>[(
[([[(<([(({<([{}][<>[]]){<{}{}><{}[]>}>(<({}[])>(<()()>))}{[[{{}()}{<><>}][{()[]}<()()>]>[{<<><>>{
[{[<{(<[([[[{[{}<>]({}())}(<<>>[[]<>])]{({<>()}{()()}){{[]()>{[]()}}}]]{[[{({}()){[][]}}((<><>))][[
<{(<[<<<{{[{{<{}<>>[[]<>]}[{()<>}[()<>]]}{[{{}[]}{()}]{<[]<>>([][])}}]({[[{}]{()<>}](({}{}))}>}{<[<({}()){()
<(<[<([({[<([<[]{}>{[]<>}]{{[]{}}{{}{}}})<<{()<>}({}{})><<[]()>{()<>]>>>(([{[]()}({}())]([[]()]<{}()>))
{[<<([[<([<<(<[]<>>){<<>()>(<><>)}><<{<><>}[{}[]]>([{}()])>>])>][{({[<<[<>]{<>()}>><<[<>()]{{
<<<[<<{[<<(({<{}<>>{{}<>}}[<[]<>>({}[])]))><{{<[[]()][(){}]><{{}()}(<>)>}[((<>{})(()<>))<[(){}][[][]]>]>>>]}
(([{{[{{<<{{[({}<>)(<>{})](<(){}><{}{}))}}([{({}())<<><>>}{<(){}>}]{{[{}{}]{(){}}}<([]<>){()()}>})
({{([{{({[{<<({}[])(()[])>[{{}()}[{}{}]]><<{()<>}{{}}>>}<[((()<>)([]{}))[{()()}(()<>)]]{[{(){}}{()
<[<{<[{({(([((<>{}){()[]}){{{}{}}{[]<>}}]{{({}<>)<{}[]>}<(()<>}(()[])>}))(<{({<><>}[[]()])<[{}()](
[[{[({{<{{{{[<<>[]]({}{})]<([]{})(<>{})>}{{({}{})[{}{}]}{<<>[]><{}>}}}{<(<<>[]>{{}{}})[(()<
[([(<{{[<[{[<(<>())<[]()>>{{{}}<[][]>}][[{[][]}<{}[]>]({{}()}[[][]])]}{{{<{}{}><{}<>>}{[()[]]<()[]>}}<<{[]{}}
[[(((({<<[<<({{}()}){<()()><<>[]>}>{((<><>)({}())){<[]{}><()[]>}}><[{<{}>}({{}{}}[<>()])]{{({}{})}[<
((<<[{{[[([[<[{}()]((){})>[{()[]}<<>{}>]]([<{}()>][{{}[]}[()]])]{({[()()][[]()]}(((){}){{}{}}))[{(<>())}
{[[<{<{[{{[(<<<>[]>([]())>{(()[])[<>{}]}){<(<>())(<>{})>{<[]<>>[{}[]]}}]({<<<>[]><{}()>>([[][]])}<[<{
[(<[<{<[([[{{{<>[]}[[]{}]>({[][]}[[][]])}{[(<>[])[[]]]({()()}({}[]))}]])<[<<<{<>{}}{[][]}>((()){()<>})>>
({<[<<[{[<({<{{}{}}{<>[]}>{{()}{[]<>}}}[<[{}<>][()[]]>])((<([]())((){})><[(){}]{{}()}>)[{<[]{}>[{}{}]}[
<[[([([(<(<[[(()[])]{[[]<>]({}{})}]>)>[<[{<<<>()>({}{})><([][])<{}{}>>}<([()[]]<(){}>)[([]{})[{}<>]]>]><[{({
[<{([([(<{<<{[()[]][()}}<<{}{}>>><<{[]<>}((){})>[{[]()}]>>}[(({{<>[]}[<>()]}<({}{})[{}[]]>)<[{<>{}}([][])
[<{([((<{({{[<<>()>{[]{}}](<[]()>[{}{}])}})(<{([<>()]{[][]})[{{}()}]}<((()[])(())){([]<>)([]<>)}>>{((({}()){
((<{{{<<[[<[[{[]<>}<{}[]>]<<(){}><<>>>]<<[[][]>[[][]]>{{<><>}{[]{}}}>>{{{{<>[]}{<>{}}}[[[]<>]<{
[<<[{[<({[[{{{{}[]}[<><>]}{<<>[]>[{}[]]}}<<(()[])[<>()]>[[<>{}]]>]]}{<({<([]())(<>())>[[{}[]][<>{}]]}){([
([<{({({{<[({(<><>){(){}}}<{[]{}}{[]())>)({<(){}>[<>[]]})]<[([{}{}]<{}[]>)[[()[]]]]{[{{}<>}]<(()<>)
[([{<{<{(<<(({[]{}}({}()))){{(<>())(<><>)}(([]{})<{}[]>)}><[<[{}{}]({}{})>]{[<[]()>(<>{})]<({}()
((<{[(<<[([<{([]{}){<><>)}[[[][]]({}())]>[[{<>()}[()<>]](({}<>)([]))]][<((()[])<()[]>)[[<>()]]>])<<{[
{[[<{<<[{{(((<{}[]>(()))[<<>()>{[][]}])){<([[]{}]{{}[]})<{{}()}{()()}>}}}[{[[<(){}>{{}()}]
{[{[<<[{[{([(<[]()>(()<>))]<{[{}<>]<{}()>}<{{}{}}[()[]]>>)[{{<{}>(())}<(<>)[<>[]]>}(<{[]<>}[()()]]<{<>{}}
[{{[{{[<{[(<{[()()][<>[]]}<([][])(()<>)>>({{{}()}[()<>]}[<[]{}><[]}]))([<<<><>>({}[])>({()[]}[<>])][([(
[({<[[<{<[(<(<[][]><[]()>){[()()]{()()}}>)[<[<()>(()[])][(<><>)]>((({}())<<>[]>))]]<[{[<<>{}>[{}<>]](<{}{}>((
<{<(<((<<[[<([<><>][()[]])(<<>{}>[<>[]])>]({<<[]>(()<>)>(((){}){<>[]})}{[[[]](<>())]<<(){}><<>[]>>
[<{(<{{<(({(<<[]()>><<()<>>>)<{<{}()><()()}}(<[][]>{{}{}})>}[{[{[][]}[()<>]]{([][])(<>[])}}[{[{}()][
{{(([[[{[{({([()()]([]<>))<[{}{}](<>())>}{{{()<>}{()()}}({()[]}[{}{}])})<([([]{})<()[]>]{<<>()>})>}[[
(((<[{<[{([[{[[]<>]{<>{}}}[[{}<>](<>{})]]<<[(){}][[]{}}>{[[][]]}>][{{[[]{}]([]{})}[([])<<>>]}
<[<[[([[[[{{{([]()){(){}}}<[[]{}](<><>)>}}]]]{([[<[(()())(()[]]][[<>{}]<<>[]>]>(<[<>]<<><>>>({{}{}}([]{
({(([<<[{{[(<({}<>)<[]<>>>(<{}<>>{(){}}))]{(({{}[]})({{}<>}((){}]))[({{}()}[()<>])<({}()){<>()}>]}}}[<
<[<({[[<<([[((<>())[<><>]){[<>[]]<<>>}]]{([<{}[]>{<><>}])})<<[<([]<>)(()<>)>(<{}()>([]<>))]>[
({({{[[<<([[({()[]}[<><>]){(()<>){<><>}}][{{<>[]}(<><>)}{[[]<>]<[]>}]])>([[<(({}[])<()>)(<()>[<>{
<([{(({<[{([<[[]<>]<()<>>><(<>())({})>][[(<>[]){[]{}}]{(<>[])<()[]>}])({[{{}()}{[]()}]}{{[[]{}]({}<>)
[([{({(({{<<<<(){}>[<>[]]>([<>()][<>[]])>[{(()())[()<>]}<{[][]}[<>{}]>]>}<[<([[]()][<>()])((<><>){[]})>
<({[{{[{{[([{({}()){{}{}}}<(()())[[][]]>][<[<>()][{}[]]><<<><>>>])([[({}{})(()<>)]{<<><>>([]<>)}]{{<()[]
<{{<{(([(<<<{{[]()}[[]()]}>{{[{}<>]<()<>>}[{{}{}}[{}[]]]}>>([([<()()>[{}<>]])([<()[]>[<>()]]{([]{})}
{<({<{[{{({[<{{}()}[()()]>{({}<>)}][<<()()>{[][]}>{<<>()><{}{}>}]})}{(<{<[()()]>}<{({}){()()}}(<[]<>><
(({{({{{<[[({((){})[<>[]]}[{<>{}}[[]()]])]{<<(<>){{}()}>[<<>>{{}{}}]>([(()[])][([]())<<>()>])}][<[[
[{[[[{<<<[<{({[]()}({}{}))[{<>}{<>[]}]}>[((<<><>>[[]<>])[[[]<>]<[][]>]){[[{}()][{}<>]]({[]
<<[{({[[[<[[[<[][]>]{{[]()}(()<>)}]{[[(){}}<{}()>][{<>{}}({}{})]}](({([]{})<()()>}[({}())]))>(<{(<(){}>)}
({<([[(<(({{<{{}[]}[[]<>]>}[<(<>[])<{}{}>><[[]<>][[]{}]>]}<[({<>{}}{{}()})(<[]()>(()<>))]([<{}()
((<[{[{[{{<[{[<>{}]}[<[][]><()>]]{{<()[]>}<[(){}]{{}<>}>}>}<(([[[]{}]<[]<>>]{[{}{}]<()[]>})[[{[]{}}{{}
[([(<([<(<<{(<{}{}>{()<>})[([]())[[]]]}[{{[]<>}((){})}<{{}[]}([]())>]><{[[<>()]]<({}())[{}<>]>}{[({}<>)
[{[<([<{<{{[[{{}{}}{<><>}]<[[]()]{[]{}}>][{({}{}){[][]}}<<{}{}>([]()]>]}[<<({}<>){()<>}>({(){}}<{}{}>)><([{}
<{((<{{<{({(([<>{}]{()<>}))(<({}[])(<>())><{{}()}[<>[]]>)}]}(<{{(({}<>)[{}{}])<<<>[]>>}}((<<<
<[<({<({{([{<<<>[]><[]{}>>(<()()>({}[]))}(((()())<{}[]>))][[[<(){}>[()()]][[{}[]]]]])}([(([{[][]}(()())][{()
[({{[<((<{{{<[[][]]({}{})>{(<>[]){[]}}}([{()[]}{[]{}}])]}{[<<[(){}]<[]<>>>{({}<>)([]())}>]
{[{(<({<[[(<<({}<>){[]{}}>(({}{})(<>[]))>({[()[]]<<>{}>}<[[][]]({}())>))(([[<>[]]([]{})][<{}>>){{[[]
{<([[(<<[(<{<<<>{}><<>>>(({}[])<()<>>)}<{<[]{}><()()>}[{(){}}]>>){<<<{{}{}}{()<>}>{{()[]}{()}>><<<<>[
<{[{(<{[<<(<<([]{})(())>[({}[]){{}()}]>)(<{[[]()]}>[{(()<>)(()<>)}{[{}<>]{()()}}])>[(({<<>[]>({}[]}}{([]
{{{([{[([[[{[{(){}}{[]()}][<<>()>]}([[[][]](<><>)])]]]{(<[(<<>[]>[{}<>])(({}[])({}{}))]{<([]{})<[][]>>((
({{[(<<<[({<[{[]<>}<{}<>>]<<()>[(){}]>>[<<<>{}>{{}[]}><({}{})>]})][(<{[(<>())]}(([<><>){[][]})
([[([{[<<{(<{<[]()>[(){}]}[(<><>){()<>}]>){{[<()()><<>[]>](<[][]>{[]<>})}<[{<>()}{()[]}]{{<><>}[<><>]}>}}{<<
<[<([<[(({<{{<(){}><<>[]>}{{<>{}}((){})}}<<[{}]({}[])}{<()()><()<>>}>>[[[{<><>}([][])](({}<>)((){}
<<[<[[<(<{(<{[[][]]{(){}}}<{{}()}<()[]>>><{<{}>}{{<>{}}[()<>]}>)}><{[<{(()[]]<()()>}<{(){}}
([([(<({{(<<{[[]<>]({}())}{{[][]}<()[]>}>>)}})(<{{<({<()[]>([]{})}[{{}<>}[<>()]])><[({<>()}{<>[
<(({{(<(<[{<[{{}[]}[<>{}])(<()<>>({}{}))>[<[<>[]]{{}}>[({}<>)[[]{}]]]}<[[{[]<>}{[]<>}]<(()()){<
{(<(([{([<({[[<>[]]{<>[]}]{{[]<>}{()}}})<[<[[]()][<>{}]>{[<>{}]<()<>>}]<(<[]>[[]()])([<>()](()<>))]>>])
{{[([[<<([(<{{{}[]}<()[]>}({[]{}}[[]{}])>(<{(){}}<(){}>><[[]{}]>))])>{{<<{<[()[]]<[]<>>>}{{(()
{{<[{[<([({<[{[][]}<<>()>]{{[]{}}({}())}]({{[]<>}}(<()<>>(<>[])))}([<[()()][[][]]>((<>{}){()<>})]
{[[{<{<<{((<<<<><>><[][]>>([()<>]<{}[]>)>{((<><>)({}()))([{}{}][<>])})<({({}[])({}())}<[()[]][{}<>]
<({{{(<{({<<(<[]<>>{{}})>[[({}<>)(()())]{[[]{}]({}{})>]>({<<()<>>[<>[]]>((()<>)([]{}))}[[([]<>)<[]<>>]{{{}<>}
{<[{([([{<[{<[<><>]([]())>((<>[])([][]))}]>(({[<[]>[[]<>]]{{{}[]}{[]{}}}}){(<{{}{}}([]<>)>)})}
(<[{{<([<[({<{{}{}}><{{}<>}([]{})>}<<[()()]{{}()}>[[[]()]({}{})]>)[[{<()[]><[]()>}<[<>()][(){}]>]]]<{(<<[]{}>
<{([[<{<([({{[<>{}]({}[])](<[][]>{{}[]})}{{{<>[]}<{}()>}<[{}[]]{{}()}>})([<[[][]]{<>()}>[<<>()>({
<{[{(<<{[<(<{{()<>}{[]()}}[([][])<{}[]>]>[[[{}<>}[()[]]][({}[]){()<>}]]){[<[{}[]][[][]]>(<{}>([][]))][[
{[{<([<<({<<(<<><>><{}[]>)[(<>()){[]{}}]>(<{{}<>}[[]<>]>([{}{}]))>}([{<(()[])[()<>]>{{()()}}}[[<[][
<{(<[{<<[[[({([]{})([]<>)}{([][])([][])})[[<[]{}><<><>>]<[{}<>]({}[])>]]<{[[{}{}]({}[])]<{[]<>}(()<>)>}[({<
(<[{([[[{[<([<{}[]>][(<>[])(()<>)])<[{[][]}([]())]>>][{([(<>())[()[]]])}({({()[]}[{}{}])}{[<[]<>>([]()
<({{[([[<<[(({{}}{{}()})<<()()><<><>>>)<[<{}[]><()<>>][<<>{}]([]{})]>]>>{<((<<[]>(())>{{()<>}}
[[[[<{[[({[{{[()[]]([]())}}<<{{}<>}(<>)>({{}{}}<[][]>)>]<{(<<>()>{(){}})[{()()}<<>>]}([{{}]<{}<>>][[()()](
(([[<(([{<({<<()()>>{<()<>><{}{}>}}{((<>()){()[]})<([]{}){()[]}>})>(<[[<[]()>{[][]>][[[]]<<>
{[<[({<(<<[[((<>())(<>[])){(<>[])(<>[])}][({[]{}}{{}<>})[<(){}>((){})]]]>>[<<{<<()[]>[[][]>>(
{<<(<([<<<((({[][]}{<><>})[{[][]}([]{})])<[<{}{}>({}())][((){})[[]()]]>)<[{(()<>)(()[])}](<[";


        #endregion
    }
}