﻿
using DA.Models.RepoResultModels;
using DM.DomainModels;
using Microsoft.EntityFrameworkCore.Query;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DA.Repositories.CommonRepositories
{

    public interface IGenericRepository<TEntity, PrimitiveType> where TEntity : Base<PrimitiveType>
    {
        SetterResult Add(TEntity entity, string createdBy);

        SetterResult Update(TEntity entity, string updatedBy);
        SetterResult UpdateMany(TEntity[] entity);
        SetterResult UpdateOnCondition(Expression<Func<TEntity, bool>> filter, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
        Task<SetterResult> UpdateOnConditionAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken);

        SetterResult Delete(TEntity entity);
        SetterResult Delete(PrimitiveType id);
        GetterResult<TEntity> GetById(PrimitiveType id);
        Task<GetterResult<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken);
        GetterResult<IEnumerable<TEntity>> GetAll();
        GetterResult<IEnumerable<TEntity>> Get(
             Expression<Func<TEntity, bool>> filter ,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy ,
            string includeProperties = ""
            );
        GetterResult<TEntity> GetSingle(
             Expression<Func<TEntity, bool>> filter ,
            string includeProperties = ""
            );
        GetterResult<IQueryable<TEntity>> GetQueryable();

        //asyncMethod
        Task<SetterResult> AddAsync(TEntity entity, string createdBy, CancellationToken cancellationToken);
        Task<SetterResult> UpdateAsync(TEntity entity, string updatedBy, CancellationToken cancellationToken);
        Task<SetterResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<SetterResult> DeleteAsync(PrimitiveType id, CancellationToken cancellationToken);
        Task<GetterResult<TEntity>> GetByIdAsync(PrimitiveType id, CancellationToken cancellationToken);


        Task<GetterResult<IEnumerable<TEntity>>> GetAsync(
              CancellationToken cancellationToken,
             Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
              
            string includeProperties = ""
          
            );

        Task<GetterResult<TEntity>> GetSingleAsync(
              CancellationToken cancellationToken,
            Expression<Func<TEntity, bool>> filter ,
              
           string includeProperties = ""
           );

       Task<GetterResult<bool>> AnyAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter);

    }
}