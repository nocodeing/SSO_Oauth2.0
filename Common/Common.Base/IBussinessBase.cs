using Factory;

namespace Common.Base
{
   /// <summary>
   /// 业务逻辑层基类接口
   /// </summary>
  public interface IBussinessBase
  {
      void JoinTransactioin(IDalFactoryRepository transactionRepository);
   
  }
}
