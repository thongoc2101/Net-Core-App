using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using NetCoreApp.Application.Interfaces;
using NetCoreApp.Application.ViewModels;
using NetCoreApp.Data.EF.Registration;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.Enums;
using NetCoreApp.Utilities.Constants;
using NetCoreApp.Utilities.Dtos;
using NetCoreApp.Utilities.Helpers;

namespace NetCoreApp.Application.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BlogViewModel Add(BlogViewModel blogVm)
        {
            var blog = Mapper.Map<BlogViewModel, Blog>(blogVm);

            if (!string.IsNullOrEmpty(blog.Tags))
            {
                var tags = blog.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.BlogTag
                        };
                        _unitOfWork.TagRepository.Add(tag);
                    }

                    var blogTag = new BlogTag { TagId = tagId };
                    blog.BlogTags.Add(blogTag);
                }
            }
            _unitOfWork.BlogRepository.Add(blog);
            return blogVm;
        }

        public void Delete(int id)
        {
            _unitOfWork.BlogRepository.Remove(id);
        }

        public List<BlogViewModel> GetAll()
        {
            return _unitOfWork.BlogRepository.FindAll(c => c.BlogTags).ProjectTo<BlogViewModel>().ToList();
        }

        public PagedResult<BlogViewModel> GetAllPaging(string keyword, int pageSize, int page = 1)
        {
            var query = _unitOfWork.BlogRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<BlogViewModel>()
            {
                Results = data.ProjectTo<BlogViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };

            return paginationSet;
        }

        public BlogViewModel GetById(int id)
        {
            return Mapper.Map<Blog, BlogViewModel>(_unitOfWork.BlogRepository.FindById(id));
        }

        public List<BlogViewModel> GetHotProduct(int top)
        {
            return _unitOfWork.BlogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<BlogViewModel>()
                .ToList();
        }

        public List<BlogViewModel> GetLastest(int top)
        {
            return _unitOfWork.BlogRepository.FindAll(x=> x.Status == Status.Active).OrderByDescending(x => x.DateCreated)
                .Take(top).ProjectTo<BlogViewModel>().ToList();
        }

        public List<BlogViewModel> GetList(string keyword)
        {
            var query = !string.IsNullOrEmpty(keyword) ?
                _unitOfWork.BlogRepository.FindAll(x => x.Name.Contains(keyword)).ProjectTo<BlogViewModel>()
                : _unitOfWork.BlogRepository.FindAll().ProjectTo<BlogViewModel>();
            return query.ToList();
        }

        public List<string> GetListByName(string name)
        {
            return _unitOfWork.BlogRepository.FindAll(x => x.Status == Status.Active
                                                && x.Name.Contains(name)).Select(y => y.Name).ToList();
        }

        public List<BlogViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _unitOfWork.BlogRepository.FindAll()
                join pt in _unitOfWork.BlogTagRepository.FindAll()
                    on p.Id equals pt.BlogId
                where pt.TagId == tagId && p.Status == Status.Active
                orderby p.DateCreated descending
                select p;

            totalRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var model = query
                .ProjectTo<BlogViewModel>();
            return model.ToList();
        }

        public List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _unitOfWork.BlogRepository.FindAll(x => x.Status == Status.Active);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BlogViewModel>().ToList();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            return _unitOfWork.TagRepository.FindAll(x => x.Type == CommonConstants.ProductTag
                                               && searchText.Contains(x.Name)).ProjectTo<TagViewModel>().ToList();
        }

        public List<TagViewModel> GetListTagById(int id)
        {
            return _unitOfWork.BlogTagRepository.FindAll(x => x.BlogId == id, c => c.Tag)
                .Select(y => y.Tag)
                .ProjectTo<TagViewModel>()
                .ToList();
        }

        public List<BlogViewModel> GetReatedBlogs(int id, int top)
        {
            return _unitOfWork.BlogRepository.FindAll(x => x.Status == Status.Active
                                                && x.Id != id)
                .OrderByDescending(x => x.DateCreated)
                .Take(top)
                .ProjectTo<BlogViewModel>()
                .ToList();
        }

        public TagViewModel GetTag(string tagId)
        {
            return Mapper.Map<Tag, TagViewModel>(_unitOfWork.TagRepository.FindSingle(x => x.Id == tagId));
        }

        public void IncreaseView(int id)
        {
            var product = _unitOfWork.BlogRepository.FindById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _unitOfWork.BlogRepository.FindAll(x => x.Status == Status.Active
                                                     && x.Name.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BlogViewModel>()
                .ToList();
        }

        public void Update(BlogViewModel blogVm)
        {
            _unitOfWork.BlogRepository.Update(Mapper.Map<BlogViewModel, Blog>(blogVm));
            if (!string.IsNullOrEmpty(blogVm.Tags))
            {
                string[] tags = blogVm.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_unitOfWork.TagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _unitOfWork.TagRepository.Add(tag);
                    }
                    _unitOfWork.BlogTagRepository.RemoveMultiple(_unitOfWork.BlogTagRepository.FindAll(x => x.Id == blogVm.Id).ToList());
                    BlogTag blogTag = new BlogTag
                    {
                        BlogId = blogVm.Id,
                        TagId = tagId
                    };
                    _unitOfWork.BlogTagRepository.Add(blogTag);
                }
            }
        }
    }
}
