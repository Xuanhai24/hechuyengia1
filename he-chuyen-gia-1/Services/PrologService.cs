using hechuyengia.Models.DiseaseDiagnose;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class PrologService
{
    private readonly HttpClient _httpClient;

    // DI sẽ inject HttpClient
    public PrologService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    // GET danh sách triệu chứng
    public async Task<List<string>?> LayDanhSachTrieuChungAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<string>>("http://localhost:8084/danhsachtrieuchung");
    }

    // POST chẩn đoán bệnh → trả List<DiagnosisResult>
    public async Task<List<Disease>?> ChuanDoanBenhAsync(List<string> trieuChung)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:8084/chuandoan", trieuChung);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Disease>>();
    }
}