﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode2021.Days
{
    public class Day5
    {
        private List<HydrothermalVenture> hydrothermalVentureList;
        public void GetResult()
        {
            PopulateInputList();
            GetPartOne();
            GetPartTwo();
        }

        private void PopulateInputList()
        {
            hydrothermalVentureList = new List<HydrothermalVenture>();

            var coordinatesSplitted = coordinates.Split(new string[] { "\r\n" },
                                           StringSplitOptions.RemoveEmptyEntries);

            foreach (var coordinateSplitted in coordinatesSplitted)
            {
                var hydrothermalVentureSplitted = coordinateSplitted.Split(new string[] { " -> " },
                                           StringSplitOptions.RemoveEmptyEntries);

                var hydrothermalVentureStart = hydrothermalVentureSplitted[0].Split(new string[] { "," },
                                           StringSplitOptions.RemoveEmptyEntries);

                var hydrothermalVentureEnd = hydrothermalVentureSplitted[1].Split(new string[] { "," },
                                           StringSplitOptions.RemoveEmptyEntries);

                var hydrothermalVenture = new HydrothermalVenture
                {
                    Start = new Point
                    {
                        X = int.Parse(hydrothermalVentureStart[0]),
                        Y = int.Parse(hydrothermalVentureStart[1]),
                    },
                    End = new Point
                    {
                        X = int.Parse(hydrothermalVentureEnd[0]),
                        Y = int.Parse(hydrothermalVentureEnd[1]),
                    },
                };

                hydrothermalVentureList.Add(hydrothermalVenture);
            }
        }

        private void GetPartOne()
        {
            //Same Index
            var hydrothermalVentureListPartOne = hydrothermalVentureList.Where(h => h.Start.X == h.End.X || h.Start.Y == h.End.Y).ToList();

            Dictionary<(int, int), HydrothermalVenturePoint> hydrothermalVenturePointDictionary = PopulateDictionary(hydrothermalVentureListPartOne);

            foreach (var hydrothermalVenture in hydrothermalVentureListPartOne)
            {
                //Check which dimension is the same.
                bool isXSameDimension = false;
                if (hydrothermalVenture.Start.X == hydrothermalVenture.End.X)
                    isXSameDimension = true;

                if (isXSameDimension)
                {
                    PopulateValuesWhenXSameDimension(hydrothermalVenturePointDictionary, hydrothermalVenture);
                }
                else
                {
                    PopulateValuesWhenYSameDimension(hydrothermalVenturePointDictionary, hydrothermalVenture);
                }

                var result = hydrothermalVenturePointDictionary.Values.Count(e => e.Occurrences >= 2);
            }
        }

        private static Dictionary<(int, int), HydrothermalVenturePoint> PopulateDictionary(List<HydrothermalVenture> hydrothermalVentureListPartOne)
        {
            var maxXStart = hydrothermalVentureListPartOne.Select(h => h.Start.X).Max();
            var maxXEnd = hydrothermalVentureListPartOne.Select(h => h.End.X).Max();
            var maxX = Math.Max(maxXStart, maxXEnd);
            var minXStart = hydrothermalVentureListPartOne.Select(h => h.Start.X).Min();
            var minXEnd = hydrothermalVentureListPartOne.Select(h => h.End.X).Min();
            var minX = Math.Min(minXStart, minXEnd);

            var maxYStart = hydrothermalVentureListPartOne.Select(h => h.Start.Y).Max();
            var maxYEnd = hydrothermalVentureListPartOne.Select(h => h.End.Y).Max();
            var maxY = Math.Max(maxYStart, maxYEnd);
            var minYStart = hydrothermalVentureListPartOne.Select(h => h.Start.Y).Min();
            var minYEnd = hydrothermalVentureListPartOne.Select(h => h.End.Y).Min();
            var minY = Math.Min(minYStart, minYEnd);


            var hydrothermalVenturePointDictionary = new Dictionary<(int, int), HydrothermalVenturePoint>();
            for (int indexX = minX; indexX <= maxX; indexX++)
            {
                for (int indexY = minY; indexY <= maxY; indexY++)
                {
                    hydrothermalVenturePointDictionary[(indexX, indexY)] = new HydrothermalVenturePoint
                    {
                        Point = new Point { X = indexX, Y = indexY },
                        Occurrences = 0
                    };
                }
            }

            return hydrothermalVenturePointDictionary;
        }

        private void GetPartTwo()
        {
            var hydrothermalVentureListPartTwo = hydrothermalVentureList.Where(h => h.Start.X == h.End.X || h.Start.Y == h.End.Y
            || (Math.Abs(h.Start.X - h.End.X) == Math.Abs(h.Start.Y - h.End.Y))) //calculate the diagonal
                .ToList();

            Dictionary<(int, int), HydrothermalVenturePoint> hydrothermalVenturePointDictionary = PopulateDictionary(hydrothermalVentureListPartTwo);

            foreach (var hydrothermalVenture in hydrothermalVentureListPartTwo)
            {
                //Check which dimension is the same.
                bool isXSameDimension = false;
                if (hydrothermalVenture.Start.X == hydrothermalVenture.End.X)
                    isXSameDimension = true;

                bool isYSameDimension = false;
                if (hydrothermalVenture.Start.Y == hydrothermalVenture.End.Y)
                    isYSameDimension = true;

                if (isXSameDimension)
                {
                    PopulateValuesWhenXSameDimension(hydrothermalVenturePointDictionary, hydrothermalVenture);
                }
                else if (isYSameDimension)
                {
                    PopulateValuesWhenYSameDimension(hydrothermalVenturePointDictionary, hydrothermalVenture);
                }
                //We manage the diagonal
                else
                {
                    PopulateValuesWhenDiagonal(hydrothermalVenturePointDictionary, hydrothermalVenture);
                }
            }

            var result = hydrothermalVenturePointDictionary.Values.Count(e => e.Occurrences >= 2);
        }

        private static void PopulateValuesWhenXSameDimension(Dictionary<(int, int), HydrothermalVenturePoint> hydrothermalVenturePointDictionary, HydrothermalVenture hydrothermalVenture)
        {
            //We parse on the y axes
            var currentXIndex = hydrothermalVenture.Start.X;
            int beginningYIndex = 0;
            int endingYIndex = 0;
            if (hydrothermalVenture.Start.Y < hydrothermalVenture.End.Y)
            {
                beginningYIndex = hydrothermalVenture.Start.Y;
                endingYIndex = hydrothermalVenture.End.Y;
            }
            else
            {
                beginningYIndex = hydrothermalVenture.End.Y;
                endingYIndex = hydrothermalVenture.Start.Y;
            }

            //Parse all the elements in the line
            for (int currentYIndex = beginningYIndex; currentYIndex <= endingYIndex; currentYIndex++)
            {
                var elem = hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)];
                elem.Occurrences = elem.Occurrences + 1;
                hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)] = elem;
            }
        }

        private static void PopulateValuesWhenYSameDimension(Dictionary<(int, int), HydrothermalVenturePoint> hydrothermalVenturePointDictionary, HydrothermalVenture hydrothermalVenture)
        {
            //We parse on the x axes
            var currentYIndex = hydrothermalVenture.Start.Y;
            int beginningXIndex = 0;
            int endingXIndex = 0;
            if (hydrothermalVenture.Start.X < hydrothermalVenture.End.X)
            {
                beginningXIndex = hydrothermalVenture.Start.X;
                endingXIndex = hydrothermalVenture.End.X;
            }
            else
            {
                beginningXIndex = hydrothermalVenture.End.X;
                endingXIndex = hydrothermalVenture.Start.X;
            }

            //Parse all the elements in the line
            for (int currentXIndex = beginningXIndex; currentXIndex <= endingXIndex; currentXIndex++)
            {
                var elem = hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)];
                elem.Occurrences = elem.Occurrences + 1;
                hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)] = elem;
            }
        }

        private static void PopulateValuesWhenDiagonal(Dictionary<(int, int), HydrothermalVenturePoint> hydrothermalVenturePointDictionary, HydrothermalVenture hydrothermalVenture)
        {
            //We go from left to right
            //Going to the right
            if (hydrothermalVenture.Start.X < hydrothermalVenture.End.X)
            {
                int beginningXIndex = hydrothermalVenture.Start.X;
                int endingXIndex = hydrothermalVenture.End.X;

                int currentYIndex = hydrothermalVenture.Start.Y;

                for (int currentXIndex = beginningXIndex; currentXIndex <= endingXIndex; currentXIndex++)
                {
                    var elem = hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)];
                    elem.Occurrences = elem.Occurrences + 1;
                    hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)] = elem;
                    currentYIndex = GetCurrentYIndex(hydrothermalVenture, currentYIndex);
                }
            }
            else
            {
                int beginningXIndex = hydrothermalVenture.Start.X;
                int endingXIndex = hydrothermalVenture.End.X;

                int currentYIndex = hydrothermalVenture.Start.Y;
                for (int currentXIndex = beginningXIndex; currentXIndex >= endingXIndex; currentXIndex--)
                {
                    var elem = hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)];
                    elem.Occurrences = elem.Occurrences + 1;
                    hydrothermalVenturePointDictionary[(currentXIndex, currentYIndex)] = elem;
                    currentYIndex = GetCurrentYIndex(hydrothermalVenture, currentYIndex);
                }
            }
        }

        private static int GetCurrentYIndex(HydrothermalVenture hydrothermalVenture, int currentYIndex)
        {
            //going down
            if (hydrothermalVenture.Start.Y < hydrothermalVenture.End.Y)
            {
                currentYIndex++;
            }
            else
            {
                //going up
                currentYIndex--;
            }

            return currentYIndex;
        }

        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class HydrothermalVenture
        {
            public Point Start { get; set; }
            public Point End { get; set; }
        }

        public class HydrothermalVenturePoint
        {
            public Point Point { get; set; }
            public int Occurrences { get; set; }
        }


        #region input


        string coordinates =
@"445,187 -> 912,654
820,46 -> 25,841
216,621 -> 458,379
955,898 -> 67,10
549,572 -> 549,520
796,107 -> 109,794
729,698 -> 338,698
11,987 -> 968,30
381,840 -> 381,409
80,467 -> 80,48
132,197 -> 132,92
343,96 -> 343,710
42,854 -> 346,550
503,56 -> 804,56
599,206 -> 60,206
702,920 -> 474,920
496,790 -> 223,517
969,579 -> 583,579
897,66 -> 604,66
484,754 -> 640,910
330,49 -> 949,49
908,132 -> 714,132
517,153 -> 97,573
317,865 -> 678,504
800,61 -> 800,51
179,242 -> 179,202
529,757 -> 529,838
288,953 -> 393,953
372,15 -> 925,568
100,85 -> 654,639
663,562 -> 737,636
576,981 -> 245,981
347,240 -> 347,928
91,119 -> 413,441
637,397 -> 330,90
534,257 -> 950,257
155,636 -> 694,97
539,274 -> 539,327
329,795 -> 824,300
103,51 -> 961,909
87,868 -> 168,787
21,250 -> 157,386
591,316 -> 670,395
722,670 -> 630,670
28,167 -> 28,489
480,210 -> 68,622
573,700 -> 354,919
186,59 -> 700,59
121,186 -> 670,186
797,985 -> 671,985
836,804 -> 335,804
701,835 -> 104,238
456,718 -> 456,964
694,891 -> 694,839
205,637 -> 205,318
40,958 -> 773,225
151,391 -> 151,779
693,894 -> 417,894
418,700 -> 656,700
196,985 -> 896,985
357,509 -> 608,258
552,558 -> 552,482
184,412 -> 170,412
122,746 -> 643,225
268,930 -> 247,930
979,820 -> 407,248
755,893 -> 845,893
706,843 -> 706,225
162,726 -> 895,726
140,888 -> 289,888
614,432 -> 903,721
272,725 -> 272,598
529,672 -> 967,234
903,989 -> 785,871
422,355 -> 422,360
313,722 -> 713,322
460,121 -> 460,151
55,944 -> 946,944
795,744 -> 221,744
816,953 -> 471,953
865,186 -> 557,186
94,976 -> 747,323
302,961 -> 811,452
361,966 -> 921,406
197,988 -> 571,988
310,905 -> 722,493
699,91 -> 272,518
295,306 -> 84,95
220,116 -> 395,291
183,364 -> 523,364
16,986 -> 16,319
54,980 -> 635,399
340,110 -> 651,421
788,76 -> 788,635
933,375 -> 458,375
12,434 -> 494,916
253,892 -> 962,183
240,508 -> 240,234
763,934 -> 506,677
308,135 -> 239,66
117,649 -> 751,15
95,535 -> 428,868
16,937 -> 902,51
547,404 -> 547,830
128,581 -> 970,581
959,810 -> 564,415
971,738 -> 378,145
919,210 -> 295,210
737,43 -> 231,43
82,577 -> 455,204
821,337 -> 570,337
688,753 -> 538,753
891,844 -> 124,844
74,957 -> 946,85
43,942 -> 43,210
100,391 -> 100,142
975,527 -> 175,527
510,844 -> 395,959
558,231 -> 558,858
839,915 -> 262,338
784,290 -> 875,199
644,824 -> 812,824
899,657 -> 500,657
263,668 -> 263,964
157,374 -> 820,374
530,301 -> 530,67
15,688 -> 15,572
216,844 -> 479,581
973,59 -> 68,964
104,92 -> 104,547
421,472 -> 421,176
887,805 -> 231,149
140,980 -> 852,980
248,602 -> 346,602
334,961 -> 334,471
892,892 -> 958,958
270,83 -> 270,135
950,105 -> 404,651
979,476 -> 930,427
416,430 -> 879,430
796,937 -> 796,415
670,679 -> 72,679
733,884 -> 733,302
745,196 -> 306,196
174,353 -> 667,846
285,978 -> 254,978
10,63 -> 936,989
242,107 -> 242,725
238,341 -> 238,800
975,102 -> 174,903
530,474 -> 530,853
931,47 -> 467,47
86,141 -> 821,141
263,15 -> 654,15
688,542 -> 378,232
826,793 -> 989,793
729,285 -> 729,192
587,915 -> 587,79
548,667 -> 877,667
15,836 -> 783,68
662,673 -> 71,82
312,681 -> 910,83
760,418 -> 491,418
175,502 -> 443,502
817,878 -> 29,90
798,569 -> 811,582
703,141 -> 743,181
941,849 -> 941,778
63,24 -> 500,461
697,183 -> 119,761
705,672 -> 152,672
150,567 -> 656,567
158,411 -> 965,411
702,872 -> 276,446
141,179 -> 741,779
533,886 -> 817,886
569,949 -> 285,949
699,764 -> 699,780
333,863 -> 805,391
861,804 -> 524,467
791,501 -> 718,501
976,265 -> 976,713
129,342 -> 339,132
322,738 -> 212,738
700,534 -> 622,456
68,314 -> 14,314
146,112 -> 215,181
170,211 -> 482,211
159,412 -> 159,32
312,939 -> 312,95
232,18 -> 912,698
950,114 -> 950,826
620,848 -> 620,11
624,288 -> 544,208
752,479 -> 752,577
784,796 -> 784,872
130,55 -> 974,899
434,82 -> 434,481
988,230 -> 892,134
159,252 -> 159,291
462,14 -> 462,977
553,981 -> 553,390
231,936 -> 51,936
58,759 -> 60,759
572,891 -> 584,891
705,303 -> 124,303
144,894 -> 970,68
865,275 -> 865,956
492,491 -> 470,491
971,15 -> 977,15
750,521 -> 33,521
913,947 -> 387,421
368,677 -> 570,677
795,186 -> 882,186
404,840 -> 678,840
187,488 -> 403,488
824,706 -> 642,706
330,541 -> 330,195
564,531 -> 774,531
271,857 -> 20,606
976,975 -> 976,843
323,341 -> 21,39
575,643 -> 267,643
827,295 -> 827,854
749,486 -> 749,780
656,716 -> 656,470
635,187 -> 417,187
503,488 -> 503,393
592,688 -> 592,567
515,408 -> 128,795
608,158 -> 780,158
677,96 -> 11,762
127,452 -> 339,452
117,985 -> 291,811
157,371 -> 157,916
640,758 -> 983,758
906,413 -> 906,776
224,842 -> 627,439
903,728 -> 903,459
358,138 -> 822,602
30,16 -> 929,915
440,900 -> 294,900
809,73 -> 987,73
55,410 -> 304,161
441,672 -> 315,672
939,40 -> 234,40
334,698 -> 309,698
572,738 -> 572,226
445,71 -> 445,468
225,660 -> 427,458
390,320 -> 449,320
507,635 -> 507,169
47,116 -> 738,807
127,14 -> 689,14
316,760 -> 316,432
831,101 -> 250,682
370,807 -> 370,898
678,186 -> 491,186
866,83 -> 539,83
518,848 -> 518,962
188,135 -> 81,28
378,226 -> 597,226
646,534 -> 141,534
275,672 -> 275,854
67,421 -> 676,421
386,323 -> 988,323
903,984 -> 10,91
37,348 -> 529,840
872,134 -> 358,648
42,826 -> 42,822
688,922 -> 21,922
47,539 -> 942,539
739,483 -> 375,847
23,217 -> 800,217
589,512 -> 589,953
292,229 -> 107,229
873,678 -> 873,770
794,295 -> 739,240
464,559 -> 936,559
685,736 -> 368,736
114,941 -> 114,307
571,643 -> 74,643
281,185 -> 273,177
497,937 -> 497,469
152,815 -> 702,815
76,43 -> 980,947
272,149 -> 101,149
934,945 -> 107,118
532,476 -> 759,476
955,942 -> 397,942
31,918 -> 931,18
790,420 -> 389,420
36,496 -> 215,317
252,209 -> 139,209
704,148 -> 719,133
413,571 -> 165,571
690,433 -> 864,607
976,417 -> 517,876
803,568 -> 443,568
335,558 -> 335,334
405,807 -> 691,521
194,482 -> 486,190
377,856 -> 377,802
313,842 -> 313,254
449,961 -> 198,710
197,916 -> 197,797
82,965 -> 959,88
371,239 -> 829,697
471,70 -> 596,70
835,144 -> 835,950
283,486 -> 506,486
147,29 -> 147,747
187,485 -> 187,195
781,144 -> 480,144
801,839 -> 925,715
415,960 -> 415,442
877,939 -> 29,91
22,118 -> 22,439
460,315 -> 450,315
982,960 -> 71,49
105,231 -> 105,331
98,174 -> 551,174
721,978 -> 38,295
167,290 -> 167,133
218,158 -> 218,908
819,812 -> 758,812
123,92 -> 123,132
66,721 -> 66,906
478,441 -> 967,930
284,58 -> 464,58
958,15 -> 37,936
310,337 -> 359,288
212,763 -> 212,373
101,279 -> 101,267
622,409 -> 106,925
318,657 -> 318,432
938,631 -> 938,650
142,881 -> 254,881
848,987 -> 848,451
686,223 -> 481,223
124,248 -> 812,248
246,267 -> 246,148
96,670 -> 324,442
645,888 -> 385,628
417,555 -> 417,858
543,495 -> 543,150
73,350 -> 440,717
459,704 -> 459,179
871,493 -> 871,764
911,34 -> 64,881
544,791 -> 703,791
447,218 -> 62,218
202,649 -> 396,649
935,916 -> 55,36
124,408 -> 477,761
608,850 -> 484,850
935,876 -> 582,876
377,612 -> 269,612
413,727 -> 365,679
64,451 -> 850,451
684,807 -> 357,807
323,364 -> 372,364
887,300 -> 419,300
837,831 -> 837,927
294,255 -> 768,729
878,23 -> 141,760
36,627 -> 157,627
824,703 -> 824,968
356,109 -> 657,109
799,266 -> 313,752
71,600 -> 650,21
564,863 -> 564,54
36,720 -> 109,720
318,488 -> 682,488
249,350 -> 979,350
560,502 -> 255,502
132,327 -> 132,246
287,906 -> 791,906
818,110 -> 818,882
937,17 -> 113,841
50,710 -> 673,87
702,952 -> 702,533
666,552 -> 611,552
612,962 -> 112,462
260,529 -> 351,529
440,313 -> 440,663
605,341 -> 405,141
277,287 -> 461,287
268,890 -> 268,92
764,526 -> 877,639
165,697 -> 832,697
240,716 -> 801,155
872,429 -> 578,429
88,816 -> 338,816
981,881 -> 981,138
457,351 -> 457,679
850,526 -> 850,447
139,449 -> 165,449
127,544 -> 127,934
160,890 -> 745,305
526,113 -> 303,336
17,500 -> 17,621
796,311 -> 181,926
260,218 -> 787,218
536,989 -> 536,261
257,826 -> 257,180
531,37 -> 531,493
961,942 -> 206,187
536,668 -> 536,868
154,967 -> 154,931
808,317 -> 808,873
487,258 -> 599,258
59,962 -> 802,219
322,945 -> 322,837
378,973 -> 33,628
668,556 -> 691,556
819,728 -> 787,728
484,261 -> 484,874
333,271 -> 278,271
733,515 -> 741,523
775,854 -> 523,602
67,215 -> 616,215
951,685 -> 951,433
372,105 -> 372,494
917,788 -> 917,23
890,584 -> 245,584
748,276 -> 893,276
733,721 -> 733,747
225,908 -> 897,908
437,140 -> 423,140
456,513 -> 136,833
413,135 -> 413,596
143,245 -> 879,981
870,639 -> 942,639
28,175 -> 696,843
393,303 -> 393,197
169,986 -> 458,986
43,44 -> 952,953
236,405 -> 60,229
266,845 -> 292,845
529,98 -> 95,532
95,658 -> 695,658
368,454 -> 112,710
506,776 -> 662,776
928,494 -> 604,170
179,138 -> 900,859
45,560 -> 408,197
655,654 -> 37,36
56,432 -> 56,456
844,614 -> 844,898
240,191 -> 240,112
639,911 -> 213,911
47,887 -> 830,104
57,50 -> 977,970
899,928 -> 111,928
962,676 -> 962,518
129,585 -> 469,245
988,775 -> 988,553
417,344 -> 842,769
468,110 -> 506,72
687,204 -> 687,345
828,553 -> 765,490
75,894 -> 75,93
26,798 -> 11,783
967,44 -> 967,478
240,481 -> 947,481
794,254 -> 162,254
502,944 -> 812,944
331,417 -> 410,417
850,275 -> 850,980
671,130 -> 671,941
240,99 -> 240,381
771,399 -> 318,399
946,11 -> 28,929
731,939 -> 824,846
268,71 -> 832,635
968,37 -> 968,642
935,365 -> 515,365
199,792 -> 932,792
32,116 -> 371,116
324,67 -> 941,67
453,181 -> 453,128
958,982 -> 115,139
962,168 -> 154,976
474,215 -> 333,215
458,675 -> 458,315
577,302 -> 300,302
704,493 -> 704,876
887,549 -> 887,439
81,328 -> 724,328
575,490 -> 670,490
576,17 -> 576,218
21,46 -> 963,988
532,235 -> 532,615
796,213 -> 796,407
55,948 -> 980,23
775,471 -> 272,471
26,138 -> 344,138
635,518 -> 915,518
727,365 -> 727,216
";
        #endregion
    }
}
