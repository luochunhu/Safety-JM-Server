
--自动创建按按天建立表的视图（如日志表，密采表）
--第一步,动态生成创建视图的sql语句

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoCreateViewSql]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AutoCreateViewSql]
GO
create proc AutoCreateViewSql
(
   @strCode nvarchar(10),
   @strTableName nvarchar(20)     
        
)
as   
	   if   exists (select *  from sys.objects where type='U' and name=@strTableName+convert(varchar(100),GETDATE(),112))
		   begin    
				 if not exists (select * from BFT_DataRight where strCode=@strCode) --如果不存在记录，则新增一条                   
					 insert into BFT_DataRight(strCode,strContent) values(@strCode,'create view [dbo].[View'+@strTableName+'] as select * from '+@strTableName+convert(varchar(100),GETDATE(),112))
				 else
                 if((select LEN(strContent) from  BFT_DataRight where strCode=@strCode) = 0 )--如果sql字段没有值，则赋值                      
				     update BFT_DataRight set strContent='create view [dbo].[View'+@strTableName+'] as select * from '+@strTableName+convert(varchar(100),GETDATE(),112)
				 else --有了以后每天自动追加sql            
				     update BFT_DataRight set strContent=Replace(strContent,'create','alter')+'    union all select * from '+@strTableName+convert(varchar(100),GETDATE(),112)
	          
		   end                                                                                                                                                                                                 
go





--第二步，动态创建视图
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AutoCreateView]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].AutoCreateView
GO

create proc AutoCreateView
(
   @strCode nvarchar(10),
   @strSql nvarchar(max)
)
as
   begin
       set @strSql = (select strContent from BFT_DataRight where strCode=@strCode)      
       exec (@strsql)  
   end 


--新建作业的时候建立两个步骤，第一个步骤执行上面的存储过程，第二个步骤执行下面的语句，可以执行多条，根据按天的表数量决定


--exec AutoCreateViewSql '001','JC_MC'
--go

--exec AutoCreateView '001',''
--go











