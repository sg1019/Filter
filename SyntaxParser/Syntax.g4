grammar Syntax;

/*
 * Parser Rules
 */
 
 expression
 :
 OPEN_PAREN expression CLOSE_PAREN              #parenExpression
 | left=expression op=binary right=expression     #binaryExpression 
 | NOT expression                                 #notExpression
 | comparison                                     #comparisonExpression
 | interval                                       #intervalExpression
 | text                                           #textExpression
 ;

 comparison
  : comparator numeric
  ;

 interval
  : boundary ((numeric SEMICOLON numeric) | (RANGE SEMICOLON RANGE)) boundary
  ;

 boundary
  : OPEN_BRACKET | CLOSE_BRACKET
  ;

 comparator
  : GT | LT | LE | GE
  ;

 binary
  : COND_AND | COND_OR | AND | OR
  ;

 text
  : QUOTED_TEXT | WORD
  ;

 numeric
  : DIGIT | UNSIGNEDINT | SIGNEDINT | DECIMAL | INFINITY
  ;

/*
 * Lexer Rules
 */

 LT                  : '<' ;
 GT                  : '>' ;
 LE                  : '<=' ;
 GE                  : '>=' ;

 NOT                 : '!' ;
 
 AND                 : '&' ;
 OR                  : '|' ;
 COND_AND            : '&&' ;
 COND_OR             : '||' ;

 WORD                : (LOWERCASE | UPPERCASE)+ ;
 QUOTED_TEXT         : ('"' .*? '"') | ('\'' .*? '\'') ;


 SIGN                : ('+'|'-') ;
 ZERO                : '0' ;
 NONZERO             : [1-9] ;
 DIGIT               : [0-9] ;
 UNSIGNEDINT         : ZERO | (NONZERO DIGIT*) ;
 SIGNEDINT           : SIGN? UNSIGNEDINT ;
 DECIMAL             : SIGNEDINT ( DEC_SEPARATOR UNSIGNEDINT )? ; 
 INFINITY            : '#' ;
 
 RANGE               : UNSIGNEDINT COLON UNSIGNEDINT ;
 
 DEC_SEPARATOR       : '.' ;
 COLON               : ':' ;
 SEMICOLON           : ';' ;
 
 OPEN_PAREN          : '(' ;
 CLOSE_PAREN         : ')' ;

 OPEN_BRACKET        : '[' ;
 CLOSE_BRACKET       : ']' ;

 WHITE_SPACE         : [ \r\n\t] -> skip ;
 EOL                 : ('\r' ? '\n') ;
 
 fragment LOWERCASE  : [a-z] ;
 fragment UPPERCASE  : [A-Z] ;

/* 
 Blank: can be use as the default operator
                Syntax: ' '

Binary operators:
                Syntax:  &           Priority: 1           IsDefault
                Syntax:  &&        Priority: 1
                Syntax:  |            Priority: 1
                Syntax:  ||          Priority: 1

Unary operators:
                Syntax:  !             Priority: 2
                Syntax:  >            Priority: 3
                Syntax:  >=         Priority: 3
                Syntax:  <            Priority: 3
                Syntax:  <=         Priority: 3

Interval operators:
                Syntax: [x;y] or ]x;y[ or [x;y[ or ]x;y]
                Infinity syntax: #
                Samples: [-123;150]   [1000000;#[   [10:23;10:30]

Operand protection: should be use as: 'Some text'
                Syntax: "
                Syntax: '

Parenthesis:
                Left: (    Right: )
*/