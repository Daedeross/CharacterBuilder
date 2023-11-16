
grammar CharacterBuilder;

// Parser

script
    : (gives | expression ) EOF;

gives
    : expression TO target;

target
    : variable + (COMMA variable +)?;

expression
    : LPAREN expression RPAREN                              #parentheticalExpression
    | has                                                   #hasExpression
    | function                                              #functionCall
    | atom                                                  #atomicExpression
    | expression POW expression                             #powerExpression
    | op=(PLUS | MINUS) expression                          #numericUnaryExpression
    | expression op=(TIMES | DIV) expression                #mulDivExpression
    | expression op=(PLUS | MINUS) expression               #addSubExpression
    | NOT expression                                        #booleanUnaryExpression
    | expression op=(GT | LT | EQ | GEQ | LEQ) expression   #comparisonExpression
    | expression op=(AND | OR) expression                   #booleanBinaryExpression
    | expression QMARK expression COLON expression          #ternaryExpression
    ;

atom : numeric_literal | boolean_literal | string_literal | variable ;
string_literal : NORMALSTRING | CHARSTRING ;
numeric_literal : (INT | FLOAT) ;
boolean_literal : TRUE | FALSE ;

has : HAS LPAREN LBRACKET trait_type RBRACKET trait_name RPAREN ;

function : NAME LPAREN argList RPAREN ;
argList : expression (',' expression)* ;


variable : trait | trait COLON property ;
property : NAME;
trait : LBRACKET trait_type RBRACKET trait_name | self | ancestor ;
trait_type : NAME;
trait_name : NAME;
self : 'me' ;
ancestor : 'owner';// | ANCESTOR LPAREN expression RPAREN ;

// LEXER

HAS : 'has' ;
TRUE : 'TRUE' | 'true';
FALSE : 'FALSE' | 'false';
LPAREN : '(' ;
RPAREN : ')' ;
LBRACKET : '[';
RBRACKET : ']';
PLUS : '+' ;
MINUS : '-' ;
TIMES : '*' ;
DIV : '/' ;
GT : '>' ;
LT : '<' ;
EQ : '=' ;
GEQ : '>=' ;
LEQ : '<=' ;
POINT : '.' ;
POW : '^' ;
AND : ('and' |'AND') ;
OR : ('or' | 'OR' ) ;
NOT : 'not';
QMARK : '?' ;
COLON : ':' ;
DQUOTE : '"' ;
TO: 'to';
COMMA: ',';
ANCESTOR : 'ancestor' ;

NAME : VALID_ID_START VALID_ID_CHAR* ;

NORMALSTRING
    : '"' ( EscapeSequence | ~('\\'|'"') )* '"'
    ;

CHARSTRING
    : '\'' ( EscapeSequence | ~('\''|'\\') )* '\''
    ;

INT
    : Digit+
    ;

HEX
    : '0' [xX] HexDigit+
    ;

FLOAT
    : Digit+ '.' Digit* ExponentPart?
    | '.' Digit+ ExponentPart?
    | Digit+ ExponentPart
    ;

HEX_FLOAT
    : '0' [xX] HexDigit+ '.' HexDigit* HexExponentPart?
    | '0' [xX] '.' HexDigit+ HexExponentPart?
    | '0' [xX] HexDigit+ HexExponentPart
    ;

fragment ExponentPart
    : [eE] [+-]? Digit+
    ;

fragment HexExponentPart
    : [pP] [+-]? Digit+
    ;

fragment EscapeSequence
    : '\\' [abfnrtvz"'\\]
    | '\\' '\r'? '\n'
    | DecimalEscape
    | HexEscape
    | UtfEscape
    ;

fragment DecimalEscape
    : '\\' Digit
    | '\\' Digit Digit
    | '\\' [0-2] Digit Digit
    ;

fragment HexEscape : '\\' 'x' HexDigit HexDigit ;
fragment UtfEscape : '\\' 'u{' HexDigit+ '}' ;

fragment VALID_ID_START : ('a' .. 'z') | ('A' .. 'Z') | '_' ;
fragment VALID_ID_CHAR : VALID_ID_START | ('0' .. '9') ;
fragment Digit : ('0' .. '9') ;
fragment HexDigit : [0-9a-fA-F] ;

WS : [ \t\u000C\r\n]+ -> skip ;