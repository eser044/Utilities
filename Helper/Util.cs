﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Util
    {
        public static List<FileInfo> ReadAllfilesOnSelectedPath(string path)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

            if (directoryInfos.Any())
                foreach (DirectoryInfo directory in directoryInfos)
                    fileInfos.AddRange(ReadAllfilesOnSelectedPath(directory.FullName));
            else
                fileInfos.AddRange(directoryInfo.GetFiles());

            return fileInfos;
        }

        public static TTarget Mapper<TSource, TTarget>(TSource source) where TTarget : new()
        {
            Func<TSource, TTarget> mapper = CreateMapper<TSource, TTarget>();
            return mapper(source);
        }

        public static Func<TSource, TTarget> CreateMapper<TSource, TTarget>() where TTarget : new()
        {
            var sourceProperties = typeof(TSource)
                .GetProperties()
                .Where(x => x.CanRead);
            var targetProperties = typeof(TTarget)
                .GetProperties()
                .Where(x => x.CanWrite)
                .ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);

            var source = Expression.Parameter(typeof(TSource), "source");
            var target = Expression.Variable(typeof(TTarget));
            var allocate = Expression.New(typeof(TTarget));
            var assignTarget = Expression.Assign(target, allocate);

            var statements = new List<Expression>();
            statements.Add(assignTarget);

            foreach (var sourceProperty in sourceProperties)
            {
                if (targetProperties.TryGetValue(sourceProperty.Name, out PropertyInfo targetProperty))
                {
                    var assignProperty = Expression.Assign(
                        Expression.Property(target, targetProperty),
                        Expression.Property(source, sourceProperty));
                    statements.Add(assignProperty);
                }
            }

            statements.Add(target);

            var body = Expression.Block(new[] { target }, statements);

            return Expression.Lambda<Func<TSource, TTarget>>(body, source).Compile();
        }

    }
}
