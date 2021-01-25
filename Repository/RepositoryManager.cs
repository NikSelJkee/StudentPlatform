using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly StudentPlatformContext _repositoryContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private IGroupRepository _groupRepository;
        private IUserRepository _userRepository;
        private ITagRepository _tagRepository;
        private IMaterialRepository _materialRepository;
        private ITestRepository _testRepository;
        private IRatingRepository _ratingRepository;

        public RepositoryManager(StudentPlatformContext repositoryContext, 
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _repositoryContext = repositoryContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IGroupRepository Group
        {
            get
            {
                if (_groupRepository == null)
                    _groupRepository = new GroupRepository(_repositoryContext);

                return _groupRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext, _userManager, _signInManager);

                return _userRepository;
            }
        }

        public ITagRepository Tag
        {
            get
            {
                if (_tagRepository == null)
                    _tagRepository = new TagRepository(_repositoryContext);

                return _tagRepository;
            }
        }

        public IMaterialRepository Material
        {
            get
            {
                if (_materialRepository == null)
                    _materialRepository = new MaterialRepository(_repositoryContext);

                return _materialRepository;
            }
        }

        public ITestRepository Test
        {
            get
            {
                if (_testRepository == null)
                    _testRepository = new TestRepository(_repositoryContext);

                return _testRepository;
            }
        }

        public IRatingRepository Rating
        {
            get
            {
                if (_ratingRepository == null)
                    _ratingRepository = new RatingRepository(_repositoryContext);

                return _ratingRepository;
            }
        }

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}
