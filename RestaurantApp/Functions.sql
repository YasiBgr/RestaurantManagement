  
create function dbo.CalculateFinalAmount  
(  
@TotalAmount DECIMal(10,2)  
)  
returns DECIMAL(10,2)  
as  
begin  
declare @Tax DECIMAL(10,2);  
declare @FinalAmount DECIMAL(10,2);  
set @Tax = @TotalAmount * 0.09;  
set @FinalAmount=@TotalAmount + @Tax;  
return @FinalAmount;  
end  