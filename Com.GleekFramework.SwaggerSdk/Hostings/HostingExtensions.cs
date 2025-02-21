using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Com.GleekFramework.SwaggerSdk
{
    /// <summary>
    /// Swagger文档拓展类
    /// </summary>
    public static partial class SwaggerHostingExtensions
    {
        /// <summary>
        /// 添加Knife4生成器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="title"></param>
        /// <param name="version"></param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public static IServiceCollection AddKnife4Gen(this IServiceCollection services, string title, string version = "v1.0.0.0", Dictionary<string, string> headers = null)
        {
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();//启用特性支持
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = title,
                    Description = "",
                    Version = version,
                    Contact = new OpenApiContact()
                    {
                        Name = SwaggerConstant.AUTHOR,
                        Email = SwaggerConstant.EMAIL,
                        Url = new Uri(SwaggerConstant.HOME)
                    }
                });

                var xmlDocumentFileList = DocumentProvider.GetXmlDocumentFileList();
                if (xmlDocumentFileList.IsNotNull())
                {
                    foreach (var documentFileName in xmlDocumentFileList)
                    {
                        options.IncludeXmlComments(documentFileName, true);
                    }
                }

                if (headers.IsNotNull())
                {
                    foreach (var header in headers)
                    {
                        options.AddSecurityDefinition(header.Value, new OpenApiSecurityScheme()
                        {
                            Name = header.Key,
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Description = "可直接在下框中输入{token}(注意中括号)"
                        });
                    }
                }
            });
            return services;
        }

        /// <summary>
        /// 使用Knife4UI界面
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseKnife4UI(this IApplicationBuilder app)
        {
            var swaggerSwitch = EnvironmentProvider.GetSwaggerSwitch();//Swagger开关配置
            if (!swaggerSwitch)
            {
                return app;
            }

            app.UseSwagger();
            app.UseKnife4UI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.EnableDeepLinking();
                options.DisplayOperationId();
                options.SwaggerEndpoint(SwaggerConstant.SWAGGERENDPOINTURL, SwaggerConstant.SWAGGERGROUPNAME);
            });
            return app;
        }
    }
}