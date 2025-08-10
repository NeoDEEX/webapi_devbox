using NeoDEEX.Data;
using NeoDEEX.Transactions;
using System.Data;

namespace bizlib1;

//
// 컴포넌트를 위한 DAC 클래스
//
public class DacLogic : FoxDacBase, IDacLogic
{
    // product 조회 메서드
    public DataSet GetProducts(string? product_name)
    {
        string query = "SELECT * FROM Products ";
        FoxDbParameterCollection parameters = this.DbAccess.CreateParamCollection();
        if (String.IsNullOrWhiteSpace(product_name) == false)
        {
            query += "WHERE product_name LIKE :product_name ";
            parameters.AddWithValue("product_name", "%" + product_name + "%");
        }
        DataSet ds = this.DbAccess.ExecuteSqlDataSet(query, parameters);
        return ds;
    }

    // product_id 값을 자동으로 생성하는 메서드
    public int GetMaxProductId()
    {
        string maxIdQuery = "SELECT COALESCE(MAX(product_id), 0) + 1 FROM Products";
        object? newIdObj = this.DbAccess.ExecuteSqlScalar(maxIdQuery, null);
        int product_id = Convert.ToInt32(newIdObj);
        return product_id;
    }

    // product 추가 메서드
    public int InsertProduct(int product_id, string product_name, int category_id, decimal unit_price)
    {
        string query = "INSERT INTO Products (product_id, product_name, category_id, unit_price, discontinued) " +
                       "VALUES (:product_id, :product_name, :category_id, :unit_price, 0)";
        FoxDbParameterCollection parameters = this.DbAccess.CreateParamCollection();
        parameters.AddWithValue("product_id", product_id);
        parameters.AddWithValue("product_name", product_name);
        parameters.AddWithValue("category_id", category_id);
        parameters.AddWithValue("unit_price", unit_price);
        return this.DbAccess.ExecuteSqlNonQuery(query, parameters);
    }

    // product 삭제 메서드
    public int DeleteProduct(int product_id)
    {
        string query = "DELETE FROM Products WHERE product_id = :product_id";
        FoxDbParameterCollection parameters = this.DbAccess.CreateParamCollection();
        parameters.AddWithValue("product_id", product_id);
        return this.DbAccess.ExecuteSqlNonQuery(query, parameters);
    }
}

// FoxTransaction 을 위한 인터페이스
public interface IDacLogic
{
    DataSet GetProducts(string? product_name);
    int GetMaxProductId();
    int InsertProduct(int product_id, string product_name, int category_id, decimal unit_price);
    int DeleteProduct(int product_id);
}
