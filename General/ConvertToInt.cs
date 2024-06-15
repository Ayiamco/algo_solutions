namespace AlgoApp.General;
public static class Converter
{
    public static int ConvertToInt(string input, bool startFromBehind = true) => startFromBehind ? ConvertToIntFromTheBack(input) : ConvertToIntFromTheFront(input); 
		
    public static decimal ConvertToDecimal(string input)
	{
		if(string.IsNullOrWhiteSpace(input)) return 0;
		
		var isNegative = false;
		if(input[0] == '-')
		{
			isNegative = true;
			input = input[1..];
		}
		
		var inputSplit = input.Split(".");
		if(inputSplit.Length > 2)
			throw new Exception ($"{input} is not a valid decimal");
		
		var leftSide = ConvertStringIntegerToDecimal(inputSplit[0]);
		var rightSide = inputSplit.Length == 1 ? 0 : ConvertStringIntegerToDecimal(inputSplit[1]);
		var returnValue = leftSide + rightSide/(Pow(10, inputSplit[1].Length)) ;
		return isNegative ? returnValue * -1 : returnValue;
	}
    
	private static int ConvertToIntFromTheBack(string stringNumber)
	{
		if(string.IsNullOrWhiteSpace(stringNumber)) return 0;
		
		var isNegative = false;
		if(stringNumber[0] == '-')
		{
			isNegative = true;
			stringNumber = stringNumber[1..];
		}
		
		var numberMap = new Dictionary<char,int>{{'0',0},{'1',1},{'2',2},{'3',3},{'4',4},{'5',5},{'6',6},{'7',7},{'8',8},{'9',9},};
		
		int returnValue = 0;
		var index = 0;
		
		while(index < stringNumber.Length)
		{
			var currentDigit = stringNumber[^(index+1)];
			if(!numberMap.TryGetValue(currentDigit, out var integerValue))
				throw new Exception ($"Cannot convert {stringNumber} to number");
		
			returnValue+= Pow(10,index) * integerValue;
				
			index++;
		}
		return isNegative ? returnValue * -1 : returnValue;
	}
	
	private static int ConvertToIntFromTheFront(string input)
	{
		var isNegative = false;
		if(input[0] == '-')
		{
			isNegative = true;
			input = input[1..];
		}
		var returnValue = 0;
		var numberMap = new Dictionary<char,int>{{'0',0},{'1',1},{'2',2},{'3',3},{'4',4},{'5',5},{'6',6},{'7',7},{'8',8},{'9',9}};
		
		foreach(var digit in input)
		{
			if(!numberMap.TryGetValue(digit, out var numericValue))
			   throw new Exception ($"{input} is not a valid number");
			
			returnValue = returnValue * 10 + numericValue;
		}
		
		return isNegative ? returnValue * -1 : returnValue;
	}
	
	private static decimal ConvertStringIntegerToDecimal(string stringNumber)
	{
		if(string.IsNullOrWhiteSpace(stringNumber)) return 0;
		var numberMap = new Dictionary<char,int>{ {'0',0},{'1',1},{'2',2},{'3',3},{'4',4},{'5',5},{'6',6},{'7',7},{'8',8},{'9',9}};
		
		int returnValue = 0;
		var index = 0;
		
		while(index < stringNumber.Length)
		{
			var currentDigit = stringNumber[^(index+1)];
			if(!numberMap.TryGetValue(currentDigit, out var integerValue))
				throw new Exception ($"Cannot convert {stringNumber} to number");
		
			returnValue+= Pow(10,index) * integerValue;
				
			index++;
		}
		return returnValue;
	}
	
	private static int Pow(int bas, int exp) => Enumerable.Repeat(bas, exp).Aggregate(1, (a, b) => a * b);
}