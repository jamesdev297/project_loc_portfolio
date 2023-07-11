using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

public class ResponsePack
{
    public int ResponseCode { get; set; }
    public string Key { get; set; }
    public string Body { get; set; }
}

public class WebCall : Singleton<WebCall>
{
    // local
    // const string root = "http://localhost:5001/project-loc-668f2/asia-northeast3/api";

    // live
    const string root = "https://asia-northeast3-project-loc-668f2.cloudfunctions.net/api";

    public void GetRequest(string uri, ConcurrentQueue<ResponsePack> responseQueue, string key)
    {
        Task.Run(async () =>
        {
            try
            {
                string url = $"{root}{uri}";

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                    Headers =
                    {
                        { "Authorization", $"Bearer {FirebaseAuth.Instance.AuthToken}"}
                    }
                })
                {
                    using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        responseQueue.Enqueue(new ResponsePack()
                        {
                            ResponseCode = (int)response.StatusCode,
                            Key = key,
                            Body = content
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e.Message);
            }

        });
    }

    public void PostRequest(string uri, string body, ConcurrentQueue<ResponsePack> responseQueue, string key)
    {
        Task.Run(async () =>
        {
            try
            {
                string url = $"{root}{uri}";

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Headers =
                    {
                        { "Authorization", $"Bearer {FirebaseAuth.Instance.AuthToken}"}
                    }
                })
                {
                    using (var stringContent = new StringContent(body, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            var content = await response.Content.ReadAsStringAsync();

                            responseQueue.Enqueue(new ResponsePack()
                            {
                                ResponseCode = (int)response.StatusCode,
                                Key = key,
                                Body = content
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e.Message);
            }
        });
    }


    public async Task<string> GetRequestAsync(string uri)
    {
        try
        {
            string url = $"{root}{uri}";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
                Headers =
                {
                    { "Authorization", $"Bearer {FirebaseAuth.Instance.AuthToken}"},
                    // { "uid", $"{FirebaseAuth.Instance.User.UserId}" }
                },
            })
            {
                using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return content;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        return "error";
    }

    public async Task<string> PostRequestAsync(string uri, string body)
    {
        try
        {
            string url = $"{root}{uri}";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Headers =
                    {
                        { "Authorization", $"Bearer {FirebaseAuth.Instance.AuthToken}"}
                    }
            })
            {
                using (var stringContent = new StringContent(body, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return content;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        return "error";
    }
}
